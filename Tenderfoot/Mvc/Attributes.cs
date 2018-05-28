using System;

namespace Tenderfoot.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InputAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class OutputAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequireInputAttribute : Attribute
    {
        public HttpMethod[] Method { get; set; }
        public RequireInputAttribute() { }
        public RequireInputAttribute(params HttpMethod[] method)
        {
            this.Method = method;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateInputAttribute : Attribute
    {
        public InputType InputType { get; set; }
        public int? Length { get; set; }
        public ValidateInputAttribute(InputType inputType)
        {
            this.InputType = inputType;
        }
        public ValidateInputAttribute(InputType inputType, int length)
        {
            this.InputType = inputType;
            this.Length = length;
        }
    }
}
