using Common.Repository;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace Common.HttpClients
{
    public class CachedHttpClientService : ICachedHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICacheRepository _cache;
        private readonly HttpClient _httpClient;

        private readonly string CACHE_KEY_PREFIX;

        public CachedHttpClientService(IHttpClientFactory httpClientFactory, ICacheRepository cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _httpClient = _httpClientFactory.CreateClient("PollyRegistryCircuitBreaker");
            CACHE_KEY_PREFIX = typeof(CachedHttpClientService).Namespace ?? "CachedHttpClientService";
        }

        public async Task<JsonObject?> FetchDataAsync(string url)
        {
            string cacheKey = $"{CACHE_KEY_PREFIX}:{url}";

            // Check cache if it is available
            if (_cache.isCacheAvailable())
            {
                var cachedValue = await _cache.GetCacheValueAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedValue))
                {
                    return JsonDocument.Parse(cachedValue).Deserialize<JsonObject>();
                }
            }

            try
            { 
                var response = await _httpClient.GetStringAsync(url);

                // Cache the response if cache is available
                if (_cache.isCacheAvailable())
                {
                    await _cache.SetCacheValueAsync(cacheKey, response);
                }

                return JsonDocument.Parse(response).Deserialize<JsonObject>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
