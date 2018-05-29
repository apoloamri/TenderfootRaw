using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using Tenderfoot.Mvc.System;
using Tenderfoot.TfSystem;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Mvc
{
    public class TfController : BaseController
    {
        [HttpPost]
        [Route("authorize")]
        public JsonResult GetAuthorization()
        {
            if (TfSettings.System.Debug)
            {
                this.Initiate<AuthorizeModel>(false);
                return this.Conclude();
            }

            this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return new JsonResult(null);
        }

        public void Initiate<Model>(bool authorize = true) where Model : TfModel, new()
        {
            try
            {
                var obj = Activator.CreateInstance(typeof(Model));

                if (!this.Authorize(authorize))
                {
                    this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
                
                this.GetBody(obj);
                this.GetQueries(obj);
                this.ModelObject = this.ModelDictionary.ToClass<Model>();
                
                if (this.ModelObject.HasLibrary)
                {
                    this.ModelObject.SetModelToLibrary();
                }

                this.GetNecessities();
                this.ModelObject.BeforeStartUp();
                this.ValidateModel();
                this.ModelObject.OnStartUp();
            }
            catch (Exception ex) when (!TfSettings.System.Debug)
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                TfDebug.WriteLog(ex);
            }
        }

        private void ValidateModel()
        {
            var validationDictionary = new Dictionary<string, object>();

            this.ValidateProperties(this.ModelObject, ref validationDictionary);

            if (this.ModelObject.Validate() is IEnumerable<ValidationResult> validationResults)
            {
                foreach (var result in validationResults)
                {
                    if (result == null)
                    {
                        continue;
                    }

                    foreach (var name in result.MemberNames)
                    {
                        this.AddModelErrors(name, result, ref validationDictionary);
                    }
                }
            }

            var jsonDictionary = new Dictionary<string, object>();

            if (validationDictionary.Count() > 0)
            {
                jsonDictionary.Add("is_valid", false);
                jsonDictionary.Add("messages", validationDictionary);
            }

            this.JsonResult = base.Json(jsonDictionary, this.JsonSettings);
        }

        private void ValidateProperties(dynamic model, ref Dictionary<string, object> validationDictionary)
        {
            if (model == null)
            {
                return;
            }

            var properties = model.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttribute<InputAttribute>() == null)
                {
                    continue;
                }

                if (property.PropertyType.GetConstructor(Type.EmptyTypes) != null &&
                    !property.PropertyType.GetType().IsAbstract)
                {
                    this.ValidateProperties(property.GetValue(model), ref validationDictionary);
                }

                var isValidated = false;
                foreach (var attribute in property.GetCustomAttributes(false))
                {
                    if (isValidated == true)
                    {
                        break;
                    }

                    var value = property.GetValue(model);

                    if (attribute is RequireInputAttribute)
                    {
                        var attr = attribute as RequireInputAttribute;
                        var result = TfValidationResult.FieldRequired(property.Name, value);
                        if (attr.Method != null)
                        {
                            if (attr.Method.ToList().Contains(this.ModelObject.Method))
                            {
                                if (result != null)
                                {
                                    this.AddModelErrors(property.Name, result, ref validationDictionary);
                                    isValidated = true;
                                }
                            }
                        }
                        else
                        {
                            if (result != null)
                            {
                                this.AddModelErrors(property.Name, result, ref validationDictionary);
                                isValidated = true;
                            }
                        }
                    }

                    if (attribute is ValidateInputAttribute && value != null)
                    {
                        var validateInputAttribute = attribute as ValidateInputAttribute;
                        var result = TfValidationResult.ValidateInput(validateInputAttribute.InputType, value, property.Name);
                        if (result != null)
                        {
                            this.AddModelErrors(property.Name, result, ref validationDictionary);
                            isValidated = true;
                        }
                    }
                }
            }
        }

        private void ExecuteMapping()
        {
            if (this.ModelObject.Mapping)
            {
                this.ModelObject.MapModel();
            }
        }

        private void ExecuteHandling()
        {
            if (this.ModelObject.Handling)
            {
                this.ModelObject.HandleModel();
            }
        }

        private void BuildModelDictionary()
        {
            var jsonDictionary = new Dictionary<string, object>();
            var properties = this.ModelObject.GetType().GetProperties();
            
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                
                foreach (var attribute in attributes)
                {
                    if (attribute is OutputAttribute)
                    {
                        var value = property.GetValue(this.ModelObject);

                        if (value != null)
                        {
                            jsonDictionary.Add(StringExtensions.ToUnderscore(property.Name), value);
                        }
                    }
                }
            }

            jsonDictionary.Add("is_valid", true);
            jsonDictionary.Add("messages", string.Empty);

            this.JsonResult = base.Json(jsonDictionary, this.JsonSettings);
        }

        [NonAction]
        public JsonResult Conclude()
        {
            try
            {
                if (this.ModelObject != null &&
                    this.ModelObject.Stop == false &&
                    this.ModelState.IsValid)
                {
                    this.ExecuteMapping();
                    this.ExecuteHandling();

                    if (this.ModelObject.HasLibrary)
                    {
                        this.ModelObject.SetLibraryToModel();
                    }

                    this.BuildModelDictionary();
                    this.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex) when (!TfSettings.System.Debug)
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                TfDebug.WriteLog(ex);
            }
            
            return this.JsonResult;
        }

        [NonAction]
        public JsonResult Validate()
        {
            if (this.ModelState.IsValid)
            {
                this.BuildModelDictionary();
            }
            this.Response.StatusCode = (int)HttpStatusCode.OK;
            return this.JsonResult;
        }

        [NonAction]
        public override ViewResult View()
        {
            return this.View(this.Conclude());
        }
    }
}
