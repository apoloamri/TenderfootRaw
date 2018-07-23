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
                    dynamic dynamicList;
                    if (property.PropertyType.GetConstructor(Type.EmptyTypes) != null)
                    {
                        dynamicList = Activator.CreateInstance(property.PropertyType);
                        var itemType = property.PropertyType.GetGenericArguments()[0];
                        if (value is List<Dictionary<string, object>>)
                        {
                            foreach (var item in value as List<Dictionary<string, object>>)
                            {
                                dynamicList.Add(GetObject(item, itemType));
                            }
                        }
                    }
                    else
                    {
                        if (property.PropertyType == typeof(String[]))
                        {
                            dynamicList = new List<string>();
                        }
                        else if (property.PropertyType == typeof(Int32[]))
                        {
                            dynamicList = new List<int>();
                        }
                        else
                        {
                            dynamicList = new List<dynamic>();
                        }
                        if (value is List<dynamic>)
                        {
                            foreach (var item in value as List<dynamic>)
                            {
                                dynamicList.Add(item);
                            }
                        }
                    }
                    if (typeof(IEnumerable<dynamic>).IsAssignableFrom(property.PropertyType))
                    {
                        if (property.PropertyType == typeof(String[]))
                        {
                            var valueList = (dynamicList as List<string>);
                            if (valueList.Count == 0)
                            {
                                value = null;
                            }
                            else
                            {
                                value = valueList.ToArray();
                            }
                        }
                        else if (property.PropertyType == typeof(Int32[]))
                        {
                            var valueList = (dynamicList as List<int>);
                            if (valueList.Count == 0)
                            {
                                value = null;
                            }
                            else
                            {
                                value = valueList.ToArray();
                            }
                        }
                        else
                        {
                            var valueList = (dynamicList as List<dynamic>);
                            if (valueList.Count == 0)
                            {
                                value = null;
                            }
                            else
                            {
                                value = valueList.ToArray();
                            }
                        }
                    }
                    else
                    {
                        value = dynamicList;
                    }
                }
                else if (value.GetType().IsNullableEnum())
                {
                    value = (Enum)value;
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
