using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vocabulary_Learning_Platform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly LeaderboardService _leaderboard;

        public LeaderboardController(LeaderboardService leaderboard)
        {
            _leaderboard = leaderboard;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _leaderboard.GetTopUsersAsync());
        }
    }
}
