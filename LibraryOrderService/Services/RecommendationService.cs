using Common.HttpClients;
using Common.Service;
using LibraryOrderService.DTOs;
using LibraryOrderService.Interfaces.Service;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;

namespace LibraryOrderService.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private readonly IPopulateBookService _populateBookService;
        private readonly ICachedHttpClientService _cachedHttpClientService;

        public RecommendationService(IConfiguration configuration, IOrderService orderService, IPopulateBookService populateBookService, ICachedHttpClientService cachedHttpClientService)
        {
            _configuration = configuration;
            _orderService = orderService;
            _populateBookService = populateBookService;
            _cachedHttpClientService = cachedHttpClientService;
        }

        public async Task<List<RecommendationByPopularityDTO>> GetRecommendationsByPopularity(int n)
        {
            var popularBooks = await _orderService.GetNMostPopularBooks(n);
            var recommendations = popularBooks.Select((o, index) => new RecommendationByPopularityDTO
            {
                BookUri = o.BookUri,
                Ranking = index + 1,
                AmountOfReads = o.TotalOrders ?? 0
            }).ToList();

            foreach (var recommendation in recommendations)
            {
                await _populateBookService.PopulateBookAsync(recommendation);
            }

            return recommendations;
        }

        /*public async Task<List<RecommendationByHistoryDTO>> GetRecommendationsByHistory(string userId, int n)
        {
            // Step 1: Fetch user's order history
            var userOrders = await _orderService.GetAll(1, 100, new QueryParamsOrderDTO { Owner = userId });

            foreach (var order in userOrders.Data)
            {
                await _populateBookService.PopulateBookAsync(order);
            }

            // Extract most read categories
            var categories = userOrders.Data
                .GroupBy(o =>
                {
                    JsonNode category;
                    if (!o.Book.TryGetPropertyValue("category", out category)) return null;
                    JsonNode categoryId;
                    if (!category.AsObject().TryGetPropertyValue("categoryId", out categoryId)) return null;
                    return categoryId.AsValue().GetValue<int>().ToString();
                })
                .Where(o => o!= null)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(n)
                .ToList();

            // Step 2: Fetch top-selling books in most-read categories
            string categoryUrl = _configuration.GetValue<string>("CategoryUrl");
            JsonObject?[] categoriesPopulated = await Task.WhenAll(categories.Select(uri => _cachedHttpClientService.FetchDataAsync($"{categoryUrl}{uri}")));

            var recommendationBookIds = categoriesPopulated.Select(c =>
            {
                JsonNode books;
                if (!c.TryGetPropertyValue("books", out books)) return null;
                return books.AsArray().AsEnumerable().Select(b =>
                {
                    JsonNode bookId;
                    if (!b.AsObject().TryGetPropertyValue("isbn", out bookId)) return null;
                    return bookId.AsValue().GetValue<string>();
                })
                .Where(b => b!= null)
                .ToList()
                .OfType<string>();

            })
            .Where(b => b != null)
            .SelectMany(l => l)
            .Distinct()
            .ToList()
            .OfType<string>();

            var stockOrdersForRecommendationBooks = await _orderService.GetStockOrders(recommendationBookIds.ToArray());

            // Create recommendations
            var recommendations = stockOrdersForRecommendationBooks
                .OrderBy(s => s.TotalOrders)
                .Select((s, index) => new RecommendationByHistoryDTO
            {
                BookUri = s.BookUri,
                Ranking = index + 1
            }).ToList();

            // Populate books
            foreach (var recommendation in recommendations)
            {
                await _populateBookService.PopulateBookAsync(recommendation);
            }

            return recommendations;
        }*/

        public async Task<List<RecommendationByHistoryDTO>> GetRecommendationsByHistory(string userId, int n)
        {
            // **Step 1: Fetch user's order history**
            var userOrders = await _orderService.GetAll(1, 100, new QueryParamsOrderDTO { Owner = userId });

            // **Populate books in user's order history**
            foreach (var order in userOrders.Data)
            {
                await _populateBookService.PopulateBookAsync(order);
            }

            // **Extract the n most read categories**
            var mostReadCategories = userOrders.Data
                .GroupBy(o => {
                    JsonNode category;
                    if (!o.Book.TryGetPropertyValue("category", out category)) return null;
                    JsonNode categoryId;
                    if (!category.AsObject().TryGetPropertyValue("categoryId", out categoryId)) return null;
                    return categoryId.AsValue().GetValue<int>().ToString();
                })
                .Where(o => o.Key != null)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(n)
                .ToList();

            // **Fetch top-selling books in most-read categories**
            string categoryUrl = _configuration.GetValue<string>("CategoryUrl");
            var categoryData = await Task.WhenAll(mostReadCategories.Select(uri => _cachedHttpClientService.FetchDataAsync($"{categoryUrl}{uri}")));

            // **Extract the book IDs of top-selling books in most-read categories**
            var topSellingBookIds = categoryData.Select(c =>
            {
                JsonNode books;
                if (!c.TryGetPropertyValue("books", out books)) return null;
                return books.AsArray().AsEnumerable().Select(b =>
                {
                    JsonNode bookId;
                    if (!b.AsObject().TryGetPropertyValue("isbn", out bookId)) return null;
                    return bookId.AsValue().GetValue<string>();
                })
                .Where(b => b != null)
                .ToList()
                .OfType<string>();

            })
            .Where(b => b != null)
            .SelectMany(l => l)
            .Distinct()
            .ToList();

            // **Fetch stock orders for top-selling books in most-read categories**
            var stockOrdersForRecommendationBooks = await _orderService.GetStockOrders(topSellingBookIds.ToArray());

            // **Create recommendations**
            var recommendations = stockOrdersForRecommendationBooks
                .OrderBy(s => s.TotalOrders)
                .Select((s, index) => new RecommendationByHistoryDTO
                {
                    BookUri = s.BookUri,
                    Ranking = index + 1
                }).ToList();

            // **Populate books in recommendations**
            foreach (var recommendation in recommendations)
            {
                await _populateBookService.PopulateBookAsync(recommendation);
            }

            return recommendations;
        }
    }
}
