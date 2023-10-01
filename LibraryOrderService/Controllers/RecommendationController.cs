using LibraryOrderService.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Security.Claims;

namespace LibraryOrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpGet("ByPopularity")]
        public async Task<IActionResult> GetRecommendationsByPopularity([FromQuery] int amount)
        {
            var recommendations = await _recommendationService.GetRecommendationsByPopularity(amount);
            return Ok(recommendations);
        }

        [HttpGet("ByHistory")]
        [Authorize]
        public async Task<IActionResult> GetRecommendationsByHistory([FromQuery] int amountPerBlock)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var recommendations = await _recommendationService.GetRecommendationsByHistory(userId, amountPerBlock);
            return Ok(recommendations);
        }
    }
}
