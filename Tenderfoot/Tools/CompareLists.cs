using System.Collections.Generic;

namespace Tenderfoot.Tools
{
    public static class CompareLists
    {
        public static bool UnorderedEqual<T>(List<T> a, List<T> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            Dictionary<T, int> d = new Dictionary<T, int>();

            foreach (T item in a)
            {
                if (d.TryGetValue(item, out int c))
                {
                    d[item] = c + 1;
                }
                else
                {
                    d.Add(item, 1);
                }
            }

            foreach (T item in b)
            {
                if (d.TryGetValue(item, out int c))
                {
                    if (c == 0)
                    {
                        return false;
                    }
                    else
                    {
                        d[item] = c - 1;
                    }
                }
                else
                {
                    return false;
                }
            }

            foreach (int v in d.Values)
            {
                if (v != 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
