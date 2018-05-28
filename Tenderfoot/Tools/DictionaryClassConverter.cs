using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Tenderfoot.TfSystem;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Tools
{
    public static class DictionaryClassConverter
    {
        private static dynamic GetObject(this Dictionary<string, object> dictionary, Type type)
        {
            dynamic model = Activator.CreateInstance(type);

            foreach (var keyValue in dictionary)
            {
                var property = type.GetProperty(keyValue.Key);

                if (property == null)
                {
                    continue;
                }

                object value = keyValue.Value;

                if (value.IsDictionary())
                {
                    value = GetObject((Dictionary<string, object>)value, property.PropertyType);
                }
                else if (value.IsList())
                {
                    dynamic dynamicList = Activator.CreateInstance(property.PropertyType);
                    var itemType = property.PropertyType.GetGenericArguments()[0];
                    
                    if (value is List<Dictionary<string, object>>)
                    {
                        foreach (var item in value as List<Dictionary<string, object>>)
                        {
                            dynamicList.Add(GetObject(item, itemType));
                        }
                    }
                    else if (value is List<dynamic>)
                    {
                        foreach (var item in value as List<dynamic>)
                        {
                            dynamicList.Add(item);
                        }
                    }
                    
                    value = dynamicList;
                }
                
                try
                {
                    property.SetValue(model, TfConvert.ChangeType(value, property.PropertyType), null);
                }
                catch
                {
                    TfDebug.WriteLog(
                        TfSettings.Logs.System,
                        $"Ignored Malformed Line - {DateTime.Now}",
                        $"Name: {keyValue.Key}{Environment.NewLine}" +
                        $"Value: {keyValue.Value}{Environment.NewLine}" +
                        $"Type: {property.PropertyType}");
                }
            }

            return model;
        }

        public static T ToClass<T>(this Dictionary<string, object> dict)
        {
            return (T)GetObject(dict, typeof(T));
        }
        
        public static Dictionary<string, object> ToDictionary(this object obj, string optionalName = null)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => $"{optionalName}{prop.Name}", prop => prop.GetValue(obj, null));
        }

        public static dynamic ToDynamic(this Dictionary<string, object> dict)
        {
            var collection = (ICollection<KeyValuePair<string, object>>)new ExpandoObject();
            foreach (var keyValue in dict)
            {
                collection.Add(keyValue);
            }
            return collection;
        }
    }
}
