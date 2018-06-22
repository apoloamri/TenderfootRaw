using Microsoft.Extensions.Caching.Memory;

namespace Tenderfoot.TfSystem
{
    public static class TfMemoryCache
    {
        private static IMemoryCache MemoryCache { get; set; }

        private static IMemoryCache GetMemoryCache()
        {
            if (MemoryCache == null)
            {
                MemoryCache = new MemoryCache(new MemoryCacheOptions());
                return MemoryCache;
            }
            return MemoryCache;
        }

        public static DataType Get<DataType>(string cacheKey)
        {
            var cache = GetMemoryCache().Get<DataType>(cacheKey);
            if (cache != null)
            {
                return cache;
            }
            return default(DataType);
        }

        public static DataType Set<DataType>(string cacheKey, DataType value)
        {
            GetMemoryCache().Set(cacheKey, value);
            return value;
        }
    }
}
