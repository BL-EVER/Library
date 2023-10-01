using Common.Service;
using LibraryOrderService.DTOs;
using LibraryOrderService.Models;

namespace LibraryOrderService.Interfaces.Service
{
    public interface IOrderService : IGenericService<Order, CreateOrderDTO, ReadOrderDTO, EditOrderDTO, EditPartialOrderDTO, QueryParamsOrderDTO, int>
    {
        public Task<ICollection<StockOrderDTO>> GetStockOrders(string[] bookUris);
        public Task<List<StockOrderDTO>> GetNMostPopularBooks(int n);
    }
}
