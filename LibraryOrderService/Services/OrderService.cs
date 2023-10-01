using AutoMapper;
using Common.Repository;
using Common.Service;
using LibraryOrderService.DTOs;
using LibraryOrderService.Interfaces.Repository;
using LibraryOrderService.Interfaces.Service;
using LibraryOrderService.Models;
using LibraryOrderService.Repositories;

namespace LibraryOrderService.Services
{
    public class OrderService : GenericService<Order, CreateOrderDTO, ReadOrderDTO, EditOrderDTO, EditPartialOrderDTO, QueryParamsOrderDTO, int>, IOrderService
    {
        IOrderRepository _repository;
        ICacheRepository _cache;
        IMapper _mapper;

        public OrderService(IOrderRepository repository, ICacheRepository cache, IMapper mapper) : base(repository, cache, mapper)
        {
            _repository = repository;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<ICollection<StockOrderDTO>> GetStockOrders(string[] bookUris)
        {
            var orders = await _repository.GetOrdersByBookUris(bookUris);

            var others = bookUris
                .Where(book => orders.Where(o => o.BookUri == book).Count() == 0)
                .Select(book => new StockOrderDTO
                {
                    BookUri = book,
                    TotalOrders = null,
                    AmountLoaned = null,
                }).ToList();

            var stocks =  orders.GroupBy(o => o.BookUri)
                .Select(g => new StockOrderDTO
                {
                    BookUri = g.Key,
                    TotalOrders = g.Count(),
                    AmountLoaned = g.Count(o => !o.IsReturned)
                }).ToList();

            stocks.AddRange(others);

            return stocks;
        }

        public async Task<List<StockOrderDTO>> GetNMostPopularBooks(int n)
        {
            return await _repository.GetNMostPopularBooks(n);
        }

    }
}
