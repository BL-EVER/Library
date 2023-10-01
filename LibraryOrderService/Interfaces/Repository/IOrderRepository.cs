using Common.Repository;
using LibraryOrderService.DTOs;
using LibraryOrderService.Models;

namespace LibraryOrderService.Interfaces.Repository
{
    public interface IOrderRepository : IGenericRepository<Order, CreateOrderDTO, ReadOrderDTO, EditOrderDTO, EditPartialOrderDTO, QueryParamsOrderDTO, int>
    {
        public Task<IEnumerable<Order>> GetOrdersByBookUris(string[] bookUris);
        public Task<List<StockOrderDTO>> GetNMostPopularBooks(int n);
    }
}
