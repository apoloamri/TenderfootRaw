using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IO = System.IO;
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
                this.Initiated = true;

                if (!this.Authorize(authorize))
                {
                    this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                if (this.ModelObject == null)
                {
                    var obj = Activator.CreateInstance(typeof(Model));
                    this.GetBody(obj);
                    this.GetQueries(obj);
                    this.ModelObject = this.ModelDictionary.ToClass<Model>();
                }
                
                this.ModelObject.BeforeStartUp();

                if (this.ModelObject.HasLibrary)
                {
                    this.ModelObject.SetModelToLibrary();
                }

                this.GetNecessities();
                this.ValidateModel();
                this.ModelObject.OnStartUp();
            }
            catch (Exception ex) when (!TfSettings.System.Debug)
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                TfDebug.WriteLog(ex);
            }
        }

        public void Upload(TfUploadModel model, bool authorize = true)
        {
            this.ModelObject = model;
            this.Initiate<TfUploadModel>(authorize);
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

                    if (attribute is InputAttribute && value != null)
                    {
                        var inputAttribute = attribute as InputAttribute;
                        if (inputAttribute.InputType.HasValue)
                        {
                            var result = TfValidationResult.Input(inputAttribute.InputType.Value, value, property.Name);
                            if (result != null)
                            {
                                this.AddModelErrors(property.Name, result, ref validationDictionary);
                                isValidated = true;
                            }
                        }
                        if (inputAttribute.Length.HasValue)
                        {
                            var result = TfValidationResult.Length(inputAttribute.Length.Value, value, property.Name);
                            if (result != null)
                            {
                                this.AddModelErrors(property.Name, result, ref validationDictionary);
                                isValidated = true;
                            }
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
            this.DictionaryResult = jsonDictionary;
        }

        [NonAction]
        public JsonResult Conclude()
        {
            if (!this.Initiated)
            {
                return this.JsonResult;
            }

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
                this.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return this.JsonResult;
        }

        [NonAction]
        public ActionResult GetFile(string path, string name, string contentType)
        {
            var fullPath = IO.Path.Combine(path, name);
            if (IO.File.Exists(fullPath))
            {
                return new FileStreamResult(new IO.FileStream(fullPath, IO.FileMode.Open), contentType);
            }
            return NotFound();
        }
        
        [NonAction]
        public new ActionResult View(string viewName)
        {
            this.Conclude();
            if (this.ModelState.IsValid)
            {
                var actionName = Convert.ToString(this.ControllerContext.RouteData.Values["action"]);
                if (!actionName.IsEmpty())
                {
                    var property = this.GetType().GetMethod(actionName);
                    if (property != null)
                    {
                        var attribute = property.GetCustomAttribute<ViewAttribute>();
                        if (attribute != null)
                        {
                            this.ViewBag.PartialView = viewName;
                            viewName = (attribute as ViewAttribute).ViewName;
                        }
                    }
                }
                if (this.DictionaryResult == null)
                {
                    this.DictionaryResult = (new DefaultModel()).ToDictionary();
                }
                this.DictionaryResult.Add("site_url", TfSettings.Web.SiteUrl);
                this.DictionaryResult.Add("api_url", TfSettings.Web.ApiUrl);
                return base.View(viewName, this.DictionaryResult);
            }
            return NotFound();
        }
        
        private bool Initiated { get; set; } = false;
        private class DefaultModel : TfModel
        {
            public override IEnumerable<ValidationResult> Validate()
            {
                return null;
            }
        }
    }
}
