using System.Text.Json.Nodes;

namespace Common.Service
{
    public interface IPopulateBookService
    {
        public Task PopulateBookAsync(object instance);
    }
}
