using System;

namespace Tenderfoot.Tools.Extensions
{
    public static class EnumExtensions
    {
        public static string GetString(this Enum enumItem)
        {
            return enumItem.ToString().Replace("_", " ");
        }
    }
}
