using System;

namespace Tenderfoot.Mvc
{
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
    public class InputAttribute : Attribute
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
    }
}
