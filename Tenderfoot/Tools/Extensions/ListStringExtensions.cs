using System.Collections.Generic;

namespace Tenderfoot.Tools.Extensions
{
    public static class ListStringExtensions
    {
        public static T[] MissingItems<T>(this List<T> a, List<T> b)
        {
            var returnList = new List<T>();

            foreach (var item in a)
            {
                if (!b.Contains(item))
                {
                    returnList.Add(item);
                }
            }

            return returnList?.ToArray();
        }
    }
}
