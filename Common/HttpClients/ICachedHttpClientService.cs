using System.Text.Json.Nodes;

namespace Common.HttpClients
{
    public interface ICachedHttpClientService
    {
        public Task<JsonObject?> FetchDataAsync(string url);
    }
}
