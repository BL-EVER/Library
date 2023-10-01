using Common.Attributes;
using System.Text.Json.Nodes;

namespace LibraryOrderService.DTOs
{
    public class RecommendationByPopularityDTO
    {
        [PopulateBook("Book")]
        public string BookUri { get; set; }
        public JsonObject? Book { get; set; }
        public int Ranking { get; set; }
        public int AmountOfReads { get; set; }
    }

    public class RecommendationByHistoryDTO
    {
        [PopulateBook("Book")]
        public string BookUri { get; set; }
        public JsonObject? Book { get; set; }
        public int Ranking { get; set; }
    }
}
