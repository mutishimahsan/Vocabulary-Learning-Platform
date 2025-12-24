using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Vocabulary_Learning_Platform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // ===============================
        // GET: api/review/today
        // ===============================
        [HttpGet("today")]
        public async Task<IActionResult> GetTodayReviews()
        {
            var userId = GetUserId();
            var words = await _reviewService.GetTodayReviews(userId);
            return Ok(words);
        }

        // ===============================
        // POST: api/review/{wordId}
        // ===============================
        [HttpPost("{wordId:guid}")]
        public async Task<IActionResult> SubmitReview(
            Guid wordId,
            [FromBody] ReviewRequest request)
        {
            var userId = GetUserId();
            await _reviewService.SubmitReview(userId, wordId, request.IsCorrect);
            return Ok(new { message = "Review submitted successfully" });
        }

        // ===============================
        // GET: api/review/progress
        // ===============================
        [HttpGet("progress")]
        public async Task<IActionResult> GetProgress()
        {
            var userId = GetUserId();
            var progress = await _reviewService.GetUserProgress(userId);
            return Ok(progress);
        }

        // ===============================
        // GET: api/review/leaderboard
        // ===============================
        [AllowAnonymous]
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            var leaderboard = await _reviewService.GetLeaderboard();
            return Ok(leaderboard);
        }

        // ===============================
        // Helpers
        // ===============================
        private Guid GetUserId()
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                // Try alternative claim names
                userIdClaim = User.FindFirstValue("sub") ??
                              User.FindFirstValue("nameid") ??
                              User.FindFirstValue("uid");
            }

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID not found in claims");
            }

            return Guid.Parse(userIdClaim!);
        }
    }

    // ===============================
    // Request DTO
    // ===============================
    public class ReviewRequest
    {
        public bool IsCorrect { get; set; }
    }
}

