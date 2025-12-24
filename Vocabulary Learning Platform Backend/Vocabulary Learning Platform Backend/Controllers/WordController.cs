using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Vocabulary_Learning_Platform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WordController : ControllerBase
    {
        private readonly IWordService _wordService;
        private readonly ILogger<WordController> _logger;

        public WordController(IWordService wordService, ILogger<WordController> logger)
        {
            _wordService = wordService;
            _logger = logger;
        }

        [HttpPost("{deckId:guid}")]
        public async Task<IActionResult> AddWord(Guid deckId, [FromBody] WordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = GetUserId();
                _logger.LogInformation($"Adding word to deck {deckId} by user {userId}");

                var word = await _wordService.AddWordAsync(userId, deckId, dto);

                return Ok(new
                {
                    success = true,
                    message = "Word created successfully",
                    word
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding word");

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message,
                    innerException = ex.InnerException?.ToString()
                });
            }
        }

        [HttpGet("deck/{deckId:guid}")]
        public async Task<IActionResult> GetWordsByDeck(Guid deckId)
        {
            try
            {
                var userId = GetUserId();
                var words = await _wordService.GetWordsByDeckAsync(userId, deckId);

                return Ok(new { success = true, words });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting words by deck");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetWordById(Guid id)
        {
            try
            {
                var userId = GetUserId();
                var word = await _wordService.GetWordByIdAsync(userId, id);

                if (word == null)
                    return NotFound(new { success = false, message = "Word not found" });

                return Ok(new { success = true, word });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word by ID");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateWord(Guid id, [FromBody] WordDto dto)
        {
            try
            {
                var userId = GetUserId();
                var updatedWord = await _wordService.UpdateWordAsync(userId, id, dto);

                return Ok(new
                {
                    success = true,
                    message = "Word updated successfully",
                    word = updatedWord
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating word");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteWord(Guid id)
        {
            try
            {
                var userId = GetUserId();
                await _wordService.DeleteWordAsync(userId, id);

                return Ok(new { success = true, message = "Word deleted successfully" });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting word");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        private Guid GetUserId()
        {
            // Get from Authorization header first
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            var bearerHeader = Request.Headers["Bearer"].FirstOrDefault();

            string token = !string.IsNullOrEmpty(authHeader) ? authHeader : bearerHeader;

            if (!string.IsNullOrEmpty(token))
            {
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }

                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userIdStr = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value ??
                               jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                if (!string.IsNullOrEmpty(userIdStr) && Guid.TryParse(userIdStr, out var userId))
                {
                    return userId;
                }
            }

            // Fallback to claims
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                             User.FindFirstValue("nameid") ??
                             User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID not found in claims");
            }

            return Guid.Parse(userIdClaim);
        }
    }
}
