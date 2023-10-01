using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;
using System.Text.Json;
using Common.Attributes;
using Common.HttpClients;

namespace Common.Service
{
    public class PopulateBookService : IPopulateBookService
    {
        private readonly IConfiguration _configuration;
        private readonly ICachedHttpClientService _cachedHttpClientService;

        public PopulateBookService(IConfiguration configuration, ICachedHttpClientService cachedHttpClientService)
        {
            _configuration = configuration;
            _cachedHttpClientService = cachedHttpClientService;
        }

        public async Task PopulateBookAsync(object instance)
        {
            if (instance == null) return;
            var properties = instance.GetType().GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(PopulateBookAttribute), false);
                foreach (PopulateBookAttribute attribute in attributes)
                {
                    var prop = instance.GetType().GetProperty(property.Name);
                    var value = prop?.GetValue(instance);

                    string bookUrl = _configuration.GetValue<string>("BookUrl");

                    if (value is string bookUri)
                    {
                        JsonObject? result = await _cachedHttpClientService.FetchDataAsync($"{bookUrl}{bookUri}");
                        instance.GetType().GetProperty(attribute.TargetPropertyName)?.SetValue(instance, result);
                    }
                    else if (value is string[] bookUris)
                    {
                        JsonObject?[] results = await Task.WhenAll(bookUris.Select(uri => _cachedHttpClientService.FetchDataAsync($"{bookUrl}{uri}")));
                        JsonArray result = new JsonArray(results);
                        instance.GetType().GetProperty(attribute.TargetPropertyName)?.SetValue(instance, result);
                    }
                }
            }
        }
    }
}