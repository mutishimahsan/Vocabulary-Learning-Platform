using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Vocabulary_Learning_Platform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeckController : ControllerBase
    {
        private readonly IDeckService _deckService;
        private readonly IUserService _userService;
        private readonly ILogger<DeckController> _logger;

        public DeckController(IDeckService deckService, IUserService userService, ILogger<DeckController> logger)
        {
            _deckService = deckService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDeck([FromBody] DeckDto dto)
        {
            try
            {
                var userId = await GetUserIdAsync();
                _logger.LogInformation($"Creating deck for user {userId}");

                var deck = await _deckService.CreateAsync(userId, dto);
                return Ok(new
                {
                    success = true,
                    message = "Deck created successfully",
                    deck
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt");
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating deck");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpGet("get-deck")]
        public async Task<IActionResult> GetMyDecks()
        {
            try
            {
                var userId = await GetUserIdAsync();
                var decks = await _deckService.GetMyDecksAsync(userId);
                return Ok(new { success = true, decks });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting decks");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpGet("get-deck-by-id/{id:guid}")]
        public async Task<IActionResult> GetDeckById(Guid id)
        {
            try
            {
                var userId = await GetUserIdAsync();
                var deck = await _deckService.GetByIdAsync(id);

                if (deck == null)
                    return NotFound(new { success = false, message = "Deck not found" });

                if (deck.OwnerId != userId)
                    return Forbid();

                return Ok(new { success = true, deck });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting deck by ID");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDeck(Guid id)
        {
            try
            {
                var userId = await GetUserIdAsync();
                await _deckService.DeleteAsync(id, userId);
                return Ok(new { success = true, message = "Deck deleted successfully" });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting deck");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpGet("test-auth")]
        [AllowAnonymous]
        public IActionResult TestAuth()
        {
            return Ok(new
            {
                success = true,
                IsAuthenticated = User.Identity?.IsAuthenticated,
                Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }

        [HttpGet("debug-user-info")]
        public IActionResult DebugUserInfo()
        {
            return Ok(new
            {
                success = true,
                userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier),
                subClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
                nameidClaim = User.FindFirstValue("nameid"),
                emailClaim = User.FindFirstValue(ClaimTypes.Email),
                roleClaim = User.FindFirstValue(ClaimTypes.Role),
                allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList(),
                isAuthenticated = User.Identity?.IsAuthenticated
            });
        }

        [HttpGet("test-headers")]
        [AllowAnonymous]
        public IActionResult TestHeaders()
        {
            var headers = new Dictionary<string, string>();

            foreach (var header in Request.Headers)
            {
                headers[header.Key] = header.Value.ToString();
            }

            return Ok(new
            {
                success = true,
                headers,
                hasAuthorizationHeader = !string.IsNullOrEmpty(Request.Headers["Authorization"].FirstOrDefault()),
                hasBearerHeader = !string.IsNullOrEmpty(Request.Headers["Bearer"].FirstOrDefault()),
                authorizationHeader = Request.Headers["Authorization"].FirstOrDefault(),
                bearerHeader = Request.Headers["Bearer"].FirstOrDefault()
            });
        }

        private async Task<Guid> GetUserIdAsync()
        {
            try
            {
                _logger.LogInformation("=== Getting User ID ===");

                if (!User.Identity?.IsAuthenticated ?? true)
                {
                    throw new UnauthorizedAccessException("User is not authenticated");
                }

                // FIRST: Try to get from Authorization header directly
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                var bearerHeader = Request.Headers["Bearer"].FirstOrDefault();

                string token = !string.IsNullOrEmpty(authHeader) ? authHeader : bearerHeader;

                if (!string.IsNullOrEmpty(token))
                {
                    // Clean token
                    if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = token.Substring("Bearer ".Length).Trim();
                    }

                    // Parse token
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    var userIdStr = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value ??
                                   jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ??
                                   jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(userIdStr) && Guid.TryParse(userIdStr, out var userId))
                    {
                        _logger.LogInformation($"Got user ID from token: {userId}");
                        return userId;
                    }
                }

                // SECOND: Try to get from claims
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                                 User.FindFirstValue("nameid") ??
                                 User.FindFirstValue(JwtRegisteredClaimNames.Sub);

                if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userIdFromClaim))
                {
                    _logger.LogInformation($"Got user ID from claim: {userIdFromClaim}");
                    return userIdFromClaim;
                }

                // THIRD: Try to get by email
                var email = User.FindFirstValue(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userService.GetUserByEmailAsync(email);
                    if (user != null)
                    {
                        _logger.LogInformation($"Got user ID by email: {user.Id}");
                        return user.Id;
                    }
                }

                throw new UnauthorizedAccessException("Could not determine user ID");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserIdAsync");
                throw;
            }
        }
    }
}