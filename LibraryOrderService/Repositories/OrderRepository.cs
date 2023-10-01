using AutoMapper;
using Common.Repository;
using LibraryOrderService.DTOs;
using LibraryOrderService.Interfaces.Repository;
using LibraryOrderService.Models;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace LibraryOrderService.Repositories
{
    public class OrderRepository : GenericRepository<Order, CreateOrderDTO, ReadOrderDTO, EditOrderDTO, EditPartialOrderDTO, QueryParamsOrderDTO, int>, IOrderRepository
    {
        private DbContext _dbContext;
        private IMapper _mapper;

        public OrderRepository(DbContext context, IMapper mapper) : base(context, mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Order>> GetOrdersByBookUris(string[] bookUris)
        {
            return await _dbContext.Set<Order>().Where(o => bookUris.Contains(o.BookUri)).ToListAsync();
        }

        public async Task<List<StockOrderDTO>> GetNMostPopularBooks(int n)
        {
            var orders = await _dbContext.Set<Order>()
                        .GroupBy(o => o.BookUri)
                        .Select(g => new StockOrderDTO
                        {
                            BookUri = g.Key,
                            TotalOrders = g.Count(),
                            AmountLoaned = g.Count(o => !o.IsReturned)
                        })
                        .OrderByDescending(x => x.TotalOrders)
                        .Take(n)
                        .ToListAsync();

            return orders;
        }
    }
}
