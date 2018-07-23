using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Mvc
{
    public abstract class TfModel<T> : TfModel, ITfModel
        where T : TfLibrary, new()
    {
        public override bool HasLibrary => true;
        public T Library { get; set; } = new T();
        
        public override IEnumerable<ValidationResult> AutoValidate()
        {
            foreach (var validateResult in this.Library.AutoValidate(this))
            {
                yield return validateResult;
            }
        }

        public void SetModelToLibrary()
        {
            this.Library.SetValuesFromModel(this);
        }

        public void SetLibraryToModel()
        {
            this.SetValuesFromModel(this.Library);
        }
    }

    public abstract class TfModel : TfBaseModel
    {
        public abstract IEnumerable<ValidationResult> Validate();
        public virtual IEnumerable<ValidationResult> AutoValidate() { return null; }

        public string Host { get; set; }
        public Controller Controller { get; set; }
        public HttpMethod Method { get; set; }
        public bool Mapping => this.Method == HttpMethod.GET;
        public bool Handling => this.Method != HttpMethod.GET;
        public bool IsGet => this.Method == HttpMethod.GET;
        public bool IsPost => this.Method == HttpMethod.POST;
        public bool IsPut => this.Method == HttpMethod.PUT;
        public bool IsDelete => this.Method == HttpMethod.DELETE;
        public bool Stop { get; private set; }
        public List<string> InvalidFields { get; set; } = new List<string>();
        
        [Output]
        [Input(InputType.All, 100)]
        public virtual string SessionKey { get; set; }
        
        [Output]
        [Input(InputType.All, 100)]
        public virtual string SessionId { get; set; }

        public string SessionIdValue => Encryption.Decrypt(this.SessionId);

        public virtual bool HasLibrary => false;
        public virtual void BeforeStartUp() { }
        public virtual void OnStartUp() { }
        public virtual void MapModel() { }
        public virtual void HandleModel() { }

        public bool IsValid(params string[] fieldNames)
        {
            foreach (var fieldName in fieldNames)
            {
                if (this.InvalidFields.Contains(fieldName))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidRequireInputs(HttpMethod? method = null)
        {
            var stringList = new List<string>();
            var properties = this.GetType().GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<RequireInputAttribute>();
                if (attribute != null)
                {
                    if (method == null)
                    {
                        stringList.Add(property.Name);
                    }
                    else
                    {
                        var requireInput = attribute as RequireInputAttribute;
                        if (requireInput.Method.Contains(method.Value))
                        {
                            stringList.Add(property.Name);
                        }
                    }
                }
            }
            return this.IsValid(stringList.ToArray());
        }

        public bool IsValidSession => this.IsValid(nameof(this.SessionId), nameof(this.SessionKey));

        public ValidationResult FieldRequired(string fieldName)
        {
            var validation = TfValidationResult.FieldRequired(fieldName, this.GetType().GetProperty(fieldName)?.GetValue(this));
            if (validation != null)
            {
                this.InvalidFields.Add(fieldName);
            }
            return validation;
        }

        public ValidationResult ValidateSession()
        {
            var validation = TfValidationResult.CheckSessionActivity(
                this.SessionId, 
                this.SessionKey, 
                nameof(this.SessionId),
                nameof(this.SessionKey));

            if (validation != null)
            {
                this.InvalidFields.Add(nameof(this.SessionId));
                this.InvalidFields.Add(nameof(this.SessionKey));
            }

            return validation;
        }

        public bool SessionActive => this.ValidateSession() == null;

        public void NewSession(string sessionId)
        {
            this.SessionId = Session.AddSession(sessionId, out string sessionKey);
            this.SessionKey = sessionKey;
        }

        public void GetSessionCookies()
        {
            if (this.SessionId.IsEmpty())
            {
                this.SessionId = this.Controller.ControllerContext.HttpContext.Request.Cookies["session_id"];
            }
            if (this.SessionKey.IsEmpty())
            {
                this.SessionKey = this.Controller.ControllerContext.HttpContext.Request.Cookies["session_key"];
            }
        }

        public void SetSessionCookies()
        {
            var cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(TfSettings.Web.SessionTimeOut)
            };
            this.Controller.ControllerContext.HttpContext.Response.Cookies.Append("session_id", this.SessionId, cookieOptions);
            this.Controller.ControllerContext.HttpContext.Response.Cookies.Append("session_key", this.SessionKey, cookieOptions);
        }

        public void StopProcess()
        {
            this.Stop = true;
        }

        public override bool HasAttribute(PropertyInfo property)
        {
            return
                property != null && 
                (property?.GetCustomAttribute<InputAttribute>(false) != null ||
                property?.GetCustomAttribute<OutputAttribute>(false) != null);
        }
    }
}
