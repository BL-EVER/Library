using LibraryOrderService.DTOs;

namespace LibraryOrderService.Interfaces.Service
{
    public interface IRecommendationService
    {
        public Task<List<RecommendationByPopularityDTO>> GetRecommendationsByPopularity(int n);
        public Task<List<RecommendationByHistoryDTO>> GetRecommendationsByHistory(string userId, int n);
    }
}
