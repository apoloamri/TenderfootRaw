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
        
        protected void Initiate<Model>(bool authorize = false) where Model : TfModel, new()
        {
            try
            {
                this.Initiated = true;
                if (!this.Authorize(authorize))
                {
                    this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
                this.GetTempModel();
                if (this.Model == null)
                {
                    var obj = Activator.CreateInstance(typeof(Model));
                    this.GetBody(obj);
                    this.GetQueries(obj);
                    this.Model = this.ModelDictionary.ToClass<Model>();
                }
                this.GetNecessities();
                this.Model.BeforeStartUp();
                this.ValidateModel();
                this.Model.OnStartUp();
            }
            catch (Exception ex) when (!TfSettings.System.Debug)
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                TfDebug.WriteLog(ex);
            }
        }

        protected void Upload(TfUploadModel model, bool authorize = false)
        {
            this.Model = model;
            this.Initiate<TfUploadModel>(authorize);
        }

        protected void SetModel()
        {
            this.TempData["StoredModel"] = this.Model;
        }

        private void GetTempModel()
        {
            if (TempData["StoredModel"] != null &&
                TempData["StoredModel"] is TfModel)
            {
                this.Model = (TfModel)this.TempData["StoredModel"];
            }
        }

        private void ValidateModel()
        {
            var validationDictionary = new Dictionary<string, object>();
            var controllerType = this.GetType();
            if (controllerType.GetCustomAttribute<GetSessionAttribute>() != null)
            {
                this.Model.GetSessionCookies();
            }
            this.ValidateProperties(this.Model, ref validationDictionary);
            if (controllerType.GetCustomAttribute<CheckActiveSessionAttribute>() != null)
            {
                if (this.Model.ValidateSession() is ValidationResult validate)
                {
                    foreach (var memberName in validate.MemberNames)
                    {
                        this.AddModelErrors(memberName, validate, ref validationDictionary);
                    }
                }
            }
            if (this.Model.Validate() is IEnumerable<ValidationResult> validationResults)
            {
                foreach (var result in validationResults)
                {
                    this.AddModelErrors(result.MemberNames?.ToArray(), result, ref validationDictionary);
                }
            }
            if (this.Model.HasLibrary)
            {
                if (this.Model.AutoValidate() is IEnumerable<ValidationResult> autoValidationResults)
                {
                    foreach (var result in autoValidationResults)
                    {
                        this.AddModelErrors(result.MemberNames?.ToArray(), result, ref validationDictionary);
                    }
                }
            }
            var modelDictionary = new Dictionary<string, object>();
            if (validationDictionary.Count() > 0)
            {
                modelDictionary.Add("is_valid", false);
                modelDictionary.Add("messages", validationDictionary);
            }
            this.JsonModel = base.Json(modelDictionary, this.JsonSettings);
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
                    if (attribute is RequireInputAttribute ||
                        attribute is InputAttribute)
                    {
                        var attr = attribute as BaseAttribute;
                        var result = attr.Validate(property.Name, value, this.Model.Method, ref isValidated);
                        this.AddModelErrors(property.Name, result, ref validationDictionary);
                    }
                }
            }
        }
        
        private void BuildModelDictionary()
        {
            var modelDictionary = new Dictionary<string, object>();
            var properties = this.Model.GetType().GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                
                foreach (var attribute in attributes)
                {
                    if (attribute is OutputAttribute)
                    {
                        var value = property.GetValue(this.Model);

                        if (value != null)
                        {
                            modelDictionary.Add(StringExtensions.ToUnderscore(property.Name), value);
                        }
                    }
                }
            }
            modelDictionary.Add("is_valid", true);
            modelDictionary.Add("messages", string.Empty);
            this.JsonModel = base.Json(modelDictionary, this.JsonSettings);
            this.ViewModel = modelDictionary;
        }

        [NonAction]
        protected JsonResult Conclude()
        {
            if (!this.Initiated)
            {
                return this.JsonModel;
            }
            try
            {
                if (this.Model != null &&
                    this.Model.Stop == false &&
                    this.ModelState.IsValid)
                {
                    if (this.Model.Mapping)
                    {
                        this.Model.MapModel();
                    }
                    if (this.Model.Handling)
                    {
                        this.Model.HandleModel();
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
            return this.JsonModel;
        }

        [NonAction]
        protected JsonResult Validate()
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
            return this.JsonModel;
        }

        [NonAction]
        protected ActionResult GetFile(string path, string name, string contentType)
        {
            var fullPath = IO.Path.Combine(path, name);
            if (IO.File.Exists(fullPath))
            {
                return new FileStreamResult(new IO.FileStream(fullPath, IO.FileMode.Open), contentType);
            }
            return NotFound();
        }
        
        [NonAction]
        protected ActionResult Page(string viewName)
        {
            this.Conclude();
            var actionName = Convert.ToString(this.ControllerContext.RouteData.Values["action"]);
            if (!actionName.IsEmpty())
            {
                if (this.ViewModel == null)
                {
                    this.ViewModel = (new DefaultModel()).ToDictionary();
                }
                this.ViewModel.Add("site_url", TfSettings.Web.SiteUrl);
                this.ViewModel.Add("api_url", TfSettings.Web.ApiUrl);
                object modelObject = this.ViewModel.ToDynamic();
                var property = this.GetType().GetMethod(actionName);
                if (property != null)
                {
                    if (property.GetCustomAttribute<ViewAttribute>() is ViewAttribute attribute)
                    {
                        this.ViewBag.Body = base.View(viewName, modelObject).ToHtml(this.HttpContext);
                        viewName = attribute.ViewName;
                    }
                }
                return base.View(viewName, modelObject);
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
