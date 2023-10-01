using Common.Model;

namespace LibraryOrderService.Models
{
    public class Order : OwnedResource
    {
        public int OrderId { get; set; }
        public string BookUri { get; set; }
        public bool IsReturned { get; set; } = false;

    }
}
