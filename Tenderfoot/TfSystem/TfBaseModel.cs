using System.Collections.Generic;
using System.Reflection;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.TfSystem
{
    public class TfBaseModel
    {
        public Dictionary<string, object> ToDictionary()
        {
            return DictionaryClassConverter.ToDictionary(this);
        }

        public void SetValuesFromModel(object model, bool setNulls = true)
        {
            if (model == null)
            {
                return;
            }

            var type = model.GetType();

            foreach (var property in type.GetProperties())
            {
                var thisProperty = this.GetProperty(property.Name);
                
                if (thisProperty == null)
                {
                    continue;
                }

                if (this.HasAttribute(thisProperty))
                {
                    var value = property.GetValue(model);
                    if (setNulls || (!value?.ToString().IsEmpty() ?? false))
                    {
                        thisProperty.SetValue(this, value);
                    }
                }
            }
        }

        public void SetValuesFromDictionary(Dictionary<string, object> dictionary)
        {
            if (dictionary == null)
            {
                return;
            }

            foreach (var item in dictionary)
            {
                var thisProperty = this.GetProperty(item.Key);

                if (thisProperty == null)
                {
                    continue;
                }

                if (this.HasAttribute(thisProperty))
                {
                    thisProperty.SetValue(this, item.Value);
                }
            }
        }

        public virtual bool HasAttribute(PropertyInfo property)
        {
            return true;
        }

        private PropertyInfo GetProperty(string propertyName)
        {
            var type = this.GetType();
            var name = propertyName.ToUnderscore();
            var thisProperty = type.GetProperty(name);

            if (thisProperty == null)
            {
                name = name.ToCamelCase();
                thisProperty = type.GetProperty(name);
            }

            if (thisProperty == null)
            {
                return null;
            }

            return thisProperty;
        }
    }
}
