using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tenderfoot.Tools.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string ToUnderscore(this string value)
        {
            return string.Concat(value.Select((x, i) =>
                i > 0 && char.IsUpper(x) ?
                "_" + x.ToString()?.ToLower() :
                x.ToString().ToLower()));
        }

        public static string ToCamelCase(this string value)
        {
            if (value.IsEmpty())
            {
                return value;
            }

            string[] array = value.Split('_');

            for (int i = 0; i < array.Length; i++)
            {
                string s = array[i];
                string first = string.Empty;
                string rest = string.Empty;

                if (s.Length > 0)
                {
                    first = Char.ToUpperInvariant(s[0]).ToString();
                }

                if (s.Length > 1)
                {
                    rest = s.Substring(1).ToLowerInvariant();
                }

                array[i] = first + rest;
            }

            string newname = string.Join("", array);
            
            return newname;
        }

        public static string ToTitleCase (this string input)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        public static Boolean ToBooleanOrDefault(this String s, Boolean Default)
        {
            return ToBooleanOrDefault((Object)s, Default);
        }
        
        public static Boolean ToBooleanOrDefault(this Object o, Boolean Default)
        {
            Boolean ReturnVal = Default;
            try
            {
                if (o != null)
                {
                    switch (o.ToString().ToLower())
                    {
                        case "yes":
                        case "true":
                        case "ok":
                        case "y":
                            ReturnVal = true;
                            break;
                        case "no":
                        case "false":
                        case "n":
                            ReturnVal = false;
                            break;
                        default:
                            ReturnVal = Boolean.Parse(o.ToString());
                            break;
                    }
                }
            }
            catch
            {
            }
            return ReturnVal;
        }

        public static string FormatFromDictionary(this string formatString, Dictionary<string, object> ValueDict)
        {
            int i = 0;
            StringBuilder newFormatString = new StringBuilder(formatString);
            Dictionary<string, int> keyToInt = new Dictionary<string, int>();
            foreach (var tuple in ValueDict)
            {
                var key = tuple.Key.ToUnderscore();
                newFormatString = newFormatString.Replace("{" + key + "}", "{" + i.ToString() + "}");
                keyToInt.Add(key, i);
                i++;
            }
            return String.Format(newFormatString.ToString(), ValueDict.OrderBy(x => keyToInt[x.Key]).Select(x => x.Value).ToArray());
        }
    }
}
