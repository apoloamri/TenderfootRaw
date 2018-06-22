using System;

namespace Tenderfoot.Mvc
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CheckActiveSessionAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class GetSessionAttribute : Attribute { }

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
    public class RequireInputAttribute : Attribute
    {
        public HttpMethod[] Method { get; set; }
        public RequireInputAttribute() { }
        public RequireInputAttribute(params HttpMethod[] method)
        {
            this.Method = method;
        }
    }
}
