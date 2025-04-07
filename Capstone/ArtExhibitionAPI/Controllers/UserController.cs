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


        [HttpGet("search")]
        public async Task<IActionResult> SearchArtworks([FromQuery] string keyword)
        {
            var artworks = await _artworkService.SearchArtworksAsync(keyword);
            return Ok(artworks);
        }


        //[Authorize(Roles = "User")]
        //[HttpPost("favorite/{artworkId}")]
        //public async Task<IActionResult> AddToFavorites(int artworkId)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //        return Unauthorized("User ID not found.");

        //    bool success = await _artworkService.AddArtworkToFavoriteAsync(userId, artworkId);
        //    if (!success)
        //        return BadRequest("Artwork is already in favorites.");

        //    return Ok("Artwork added to favorites.");
        //}
        [Authorize(Roles = "User")]
        [HttpPost("favorite/{artworkId}")]
        public async Task<IActionResult> AddToFavorites(int artworkId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found.");

            try
            {
                bool success = await _artworkService.AddArtworkToFavoriteAsync(userId, artworkId);

                if (!success)
                    return BadRequest("⚠️ Artwork is already in your favorites!");

                return Ok("✅ Artwork added to favorites!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Optional: log error here
                return StatusCode(500, $"❌ An unexpected error occurred: {ex.Message}");
            }
        }



        // Remove Artwork from Favorites (Requires Auth)
        [Authorize(Roles = "User,Artist")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            // Retrieve the user ID from the logged-in user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            // Fetch the user profile data using the IUserService
            var userProfile = await _userService.GetUserProfileAsync(userId);
            if (userProfile == null)
                return NotFound("User profile not found.");

            return Ok(userProfile); // Return the profile data (e.g., name, email, etc.)
        }
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
        [Authorize(Roles = "User")]
        [HttpGet("favorite-artworks")]
        public async Task<IActionResult> GetUserFavoriteArtworks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found.");

            var favoriteArtworks = await _artworkService.GetUserFavoriteArtworksAsync(userId);
            return Ok(favoriteArtworks);
        }
        
        [HttpGet("artworks")]
        public async Task<ActionResult<List<ArtworkInfoDTO>>> GetAllArtworks()
        {
            var artworks = await _artworkService.GetAllArtworksAsync();
            return Ok(artworks);
        }


    }
}
