using StackExchange.Redis;

namespace Common.Repository
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly StackExchange.Redis.IDatabase _database;

        public CacheRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = connectionMultiplexer.GetDatabase();
        }

        public bool isCacheAvailable()
        {
            
            return _connectionMultiplexer.IsConnected;
        }

        public async Task<string> GetCacheValueAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task SetCacheValueAsync(string key, string value, int minutesLifespan = 1)
        {
            await _database.StringSetAsync(key, value, TimeSpan.FromMinutes(minutesLifespan));
        }

        public async Task RemoveCacheValueAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            // get the server we are using
            var options = ConfigurationOptions.Parse(_database.Multiplexer.Configuration);

            var server = _database.Multiplexer.GetServer(options.EndPoints.First());

            // find keys that match the prefix
            var keys = server.Keys(pattern: $"{prefix}*");

            // delete each key that matches the prefix
            foreach (var key in keys)
            {
                await _database.KeyDeleteAsync(key);
            }
        }
    }
}
