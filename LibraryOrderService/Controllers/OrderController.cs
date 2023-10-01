using Common.Controller;
using LibraryOrderService.DTOs;
using LibraryOrderService.Interfaces.Service;
using LibraryOrderService.Models;
using LibraryOrderService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryOrderService.Controllers
{
    public class OrderController : GenericController<Order, CreateOrderDTO, ReadOrderDTO, EditOrderDTO, EditPartialOrderDTO, QueryParamsOrderDTO, int>
    {
        IOrderService _service;

        public OrderController(IOrderService service) : base(service)
        {
            _service = service;
        }

        [HttpPut]
        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<IActionResult> Update(int id, EditOrderDTO editDto)
        {
            return Task.FromResult<IActionResult>(BadRequest("PUT method is disabled in this controller."));
        }

        [HttpGet("stock")]
        public async Task<ActionResult<ICollection<StockOrderDTO>>> GetStockOrders([FromQuery] string[] ISBNs)
        {
            var stockOrders = await _service.GetStockOrders(ISBNs);

            return Ok(stockOrders);
        }
    }
}
