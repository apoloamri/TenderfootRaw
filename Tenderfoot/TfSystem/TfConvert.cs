using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tenderfoot.Tools;

namespace Tenderfoot.TfSystem
{
    public class TfConvert
    {
        public static object ChangeType(object value, Type conversion)
        {
            if (!CanChangeType(value, conversion))
            {
                return value;
            }

            Type type = Nullable.GetUnderlyingType(conversion) ?? conversion;

            return (value == null) ? null : Convert.ChangeType(value, type);
        }

        public static bool CanChangeType(object value, Type conversionType)
        {
            if (conversionType == null)
            {
                return false;
            }

            if (value == null)
            {
                return false;
            }

            IConvertible convertible = value as IConvertible;

            if (convertible == null)
            {
                return false;
            }

            return true;
        }
    }
}
