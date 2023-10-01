using Common.Attributes;
using Common.DTOs;
using System.Text.Json.Nodes;

namespace LibraryOrderService.DTOs
{
    public class CreateOrderDTO : CreateOwnedResourceDTO
    {
        public string BookUri { get; set; }
        public bool IsReturned { get; private set; } = false;
    }

    public class EditOrderDTO : EditOwnedResourceDTO { }
    public class EditPartialOrderDTO : EditOwnedResourceDTO
    {
        public bool IsReturned { get; set; }
    }

    public class QueryParamsOrderDTO : QueryParamsOwnedResourceDTO
    {
        public string? BookUri { get; set; } = null;
        public bool? IsReturned { get; private set; } = null;
        public override string? ToString()
        {
            return $"{BookUri}-{IsReturned}";
        }
    }

    public class ReadOrderDTO : ReadOwnedResourceDTO
    {
        public int OrderId { get; set; }
        [PopulateBook("Book")]
        public string BookUri { get; set; }
        public JsonObject? Book { get; set; }
        public bool IsReturned { get; set; }
    }

    public class StockOrderDTO
    {
        [PopulateBook("Book")]
        public string BookUri { get; set; }
        public JsonObject? Book { get; set; }
        public int? TotalOrders { get; set; }
        public int? AmountLoaned { get; set; }
    }
}
