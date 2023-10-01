using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repository
{
    public interface ICacheRepository
    {
        public bool isCacheAvailable();
        public Task<string> GetCacheValueAsync(string key);
        public Task SetCacheValueAsync(string key, string value, int minutesLifespan = 1);
        public Task RemoveCacheValueAsync(string key);
        public Task RemoveByPrefixAsync(string prefix);
    }
}
