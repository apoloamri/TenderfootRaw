using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Tenderfoot.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BaseAttribute : Attribute
    {
        public virtual ValidationResult Validate(string propertyName, object value, HttpMethod method, ref bool isValidated)
        {
            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CheckActiveSessionAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class GetSessionAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class InputAttribute : BaseAttribute
    {
        public InputType? InputType { get; set; }
        public int? Length { get; set; }
        public InputAttribute() { }
        public InputAttribute(InputType inputType)
        {
            this.InputType = inputType;
        }
        public InputAttribute(InputType inputType, int length)
        {
            this.InputType = inputType;
            this.Length = length;
        }
        public override ValidationResult Validate(string propertyName, object value, HttpMethod method, ref bool isValidated)
        {
            if (this.InputType.HasValue)
            {
                var result = TfValidationResult.Input(this.InputType.Value, value, propertyName);
                if (result != null)
                {
                    isValidated = true;
                    return result;
                }
            }
            if (this.Length.HasValue)
            {
                var result = TfValidationResult.Length(this.Length.Value, value, propertyName);
                if (result != null)
                {
                    isValidated = true;
                    return result;
                }
            }
            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OutputAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class ViewAttribute : Attribute
    {
        public string ViewName { get; set; }
        public ViewAttribute(string viewName)
        {
            this.ViewName = viewName;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequireInputAttribute : BaseAttribute
    {
        public HttpMethod[] Method { get; set; }
        public RequireInputAttribute() { }
        public RequireInputAttribute(params HttpMethod[] method)
        {
            this.Method = method;
        }
        public override ValidationResult Validate(string propertyName, object value, HttpMethod method, ref bool isValidated)
        {
            if (this.Method != null)
            {
                var result = TfValidationResult.FieldRequired(propertyName, value);
                if (this.Method.ToList().Contains(method))
                {
                    if (result != null)
                    {
                        isValidated = true;
                        return result;
                    }
                }
            }
            else
            {
                var result = TfValidationResult.FieldRequired(propertyName, value);
                if (result != null)
                {
                    isValidated = true;
                    return result;
                }
            }
            return null;
        }
    }
}
