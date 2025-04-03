using ArtExhibition.Application.Model.Identity;
using ArtExhibition.Application.Model;
using ArtExhibition.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtExhibitionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IArtWorkService _artworkService;

        public UserController(IUserService userService, IArtWorkService artworkService)
        {
            _userService = userService;
            _artworkService = artworkService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginUserAsync(LoginRequest loginRequest)
        {
            var response = await _userService.LoginUserAsync(loginRequest);
            return Ok(response);
        }

        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            return Ok("User profile data.");
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchArtworks([FromQuery] string keyword)
        {
            var artworks = await _artworkService.SearchArtworksAsync(keyword);
            return Ok(artworks);
        }

        // ✅ Add Artwork to Favorites (Requires Auth)
        [Authorize(Roles = "User,Artist")]
        [HttpPost("favorite/{artworkId}")]
        public async Task<IActionResult> AddToFavorites(int artworkId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found.");

            bool success = await _artworkService.AddArtworkToFavoriteAsync(userId, artworkId);
            if (!success)
                return BadRequest("Artwork is already in favorites.");

            return Ok("Artwork added to favorites.");
        }

        // ✅ Remove Artwork from Favorites (Requires Auth)
        [Authorize(Roles = "User,Artist")]
        [HttpDelete("favorite/{artworkId}")]
        public async Task<IActionResult> RemoveFromFavorites(int artworkId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found.");

            bool success = await _artworkService.RemoveArtworkFromFavoriteAsync(userId, artworkId);
            if (!success)
                return NotFound("Artwork not found in favorites.");

            return Ok("Artwork removed from favorites.");
        }
        [Authorize(Roles = "User,Artist")]
        [HttpGet("favorite-artworks")]
        public async Task<IActionResult> GetUserFavoriteArtworks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found.");

            var favoriteArtworks = await _artworkService.GetUserFavoriteArtworksAsync(userId);
            return Ok(favoriteArtworks);
        }

    }
}
