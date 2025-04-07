using ArtExhibition.Application.Model;
using ArtExhibition.Domain.Interface;
using ArtExhibition.Domain.Model;
using ArtExhibition.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArtExhibitionAPI.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistRepository _artworkRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IGalleryRepository _galleryRepository;

        public ArtistController(IArtistRepository artworkRepository, IWebHostEnvironment environment, IGalleryRepository galleryRepository)
        {
            _artworkRepository = artworkRepository;
            _environment = environment;
            _galleryRepository = galleryRepository;
        }

        [HttpGet("dashboard")]
        public IActionResult GetArtistDashboard()
        {
            return Ok("Artist dashboard data.");
        }
        [Authorize(Roles = "Artist")]
        [HttpPost("upload-artwork")]
        public async Task<IActionResult> UploadArtwork([FromForm] ArtWorkUploadRequest model)
        {
            if (model.Image == null || model.Image.Length == 0)
                return BadRequest("Image file is required.");

            var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var artist = await _artworkRepository.GetArtistByUserIdAsync(userIdClaim);
            if (artist == null)
                return BadRequest("Artist not found for this user.");

            int artistId = artist.ArtistID;

            string webRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(webRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Image.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(fileStream);
            }

            var artwork = new Artwork
            {
                Title = model.Title,
                Description = model.Description,
                ImageURL = $"/uploads/{uniqueFileName}",
                ArtistID = artistId,
                CreationDate = DateTime.UtcNow
            };

            var addedArtwork = await _artworkRepository.AddArtworkAsync(artwork);
            return CreatedAtAction(nameof(UploadArtwork), new { id = addedArtwork.ArtworkID }, addedArtwork);
        }
        [Authorize(Roles = "Artist")]

        // Update Artwork Title & Description
        [HttpPut("update-artwork/{id}")]
        public async Task<IActionResult> UpdateArtwork(int id, [FromBody] ArtworkUpdateRequest model)
        {
            if (string.IsNullOrEmpty(model.Title) && string.IsNullOrEmpty(model.Description))
                return BadRequest("Title or Description must be provided for update.");

            var artwork = await _artworkRepository.GetArtworkByIdAsync(id);
            if (artwork == null)
                return NotFound("Artwork not found.");

            // Get the ArtistID from the authenticated user
            var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var artist = await _artworkRepository.GetArtistByUserIdAsync(userIdClaim);
            if (artist == null || artwork.ArtistID != artist.ArtistID)
                return Forbid("You are not authorized to update this artwork.");

            // Update artwork properties
            artwork.Title = model.Title ?? artwork.Title;
            artwork.Description = model.Description ?? artwork.Description;

            await _artworkRepository.UpdateArtworkAsync(artwork);
            return Ok("Artwork updated successfully.");
        }

        [Authorize(Roles = "Artist")]
        //  Delete Artwork
        [HttpDelete("delete-artwork/{id}")]
        public async Task<IActionResult> DeleteArtwork(int id)
        {
            var artwork = await _artworkRepository.GetArtworkByIdAsync(id);
            if (artwork == null)
                return NotFound("Artwork not found.");

            // Get the ArtistID from the authenticated user
            var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var artist = await _artworkRepository.GetArtistByUserIdAsync(userIdClaim);
            if (artist == null || artwork.ArtistID != artist.ArtistID)
                return Forbid("You are not authorized to delete this artwork.");

            await _artworkRepository.DeleteArtworkAsync(artwork);
            return Ok("Artwork deleted successfully.");
        }
        //[Authorize(Roles = "Artist")]
        //[HttpPost("create-gallery")]
        //public async Task<IActionResult> CreateGallery([FromBody] GalleryRequest model)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //        return Unauthorized("User ID not found in token.");

        //    var artist = await _artworkRepository.GetArtistByUserIdAsync(userId);
        //    if (artist == null)
        //        return BadRequest("Artist not found.");

        //    if (model.ArtworkIds.Count > 20)
        //        return BadRequest("A gallery can have a maximum of 20 artworks.");

        //    // Validate if all artworks belong to the artist
        //    foreach (var artworkId in model.ArtworkIds)
        //    {
        //        var artwork = await _artworkRepository.GetArtworkByIdAsync(artworkId);
        //        if (artwork == null || artwork.ArtistID != artist.ArtistID)
        //        {
        //            return Forbid($"Artwork with ID {artworkId} does not belong to you.");
        //        }
        //    }

        //    var gallery = new Gallery
        //    {
        //        Name = model.Name,
        //        Description = model.Description,
        //        Location = model.Location,
        //        ArtistID = artist.ArtistID

        //    };

        //    var createdGallery = await _galleryRepository.CreateGalleryAsync(gallery, model.ArtworkIds);
        //    return CreatedAtAction(nameof(CreateGallery), new { id = createdGallery.GalleryID }, createdGallery);
        //}
        [Authorize(Roles = "Artist")]
        [HttpPost("create-gallery")]
        public async Task<IActionResult> CreateGallery([FromBody] GalleryRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var artist = await _artworkRepository.GetArtistByUserIdAsync(userId);
            if (artist == null)
                return BadRequest("Artist not found.");

            if (model.ArtworkIds.Count > 20)
                return BadRequest("A gallery can have a maximum of 20 artworks.");

            // Validate if all artworks belong to the artist
            foreach (var artworkId in model.ArtworkIds)
            {
                var artwork = await _artworkRepository.GetArtworkByIdAsync(artworkId);
                if (artwork == null || artwork.ArtistID != artist.ArtistID)
                {
                    return Forbid($"Artwork with ID {artworkId} does not belong to you.");
                }
            }

            var gallery = new Gallery
            {
                Name = model.Name,
                Description = model.Description,
                Location = model.Location,
                ArtistID = artist.ArtistID
            };

            var createdGallery = await _galleryRepository.CreateGalleryAsync(gallery, model.ArtworkIds);
            return CreatedAtAction(nameof(CreateGallery), new { id = createdGallery.GalleryID }, createdGallery);
        }


        [Authorize(Roles = "Artist")]
        [HttpPut("update-gallery/{id}")]
        public async Task<IActionResult> UpdateGallery(int id, [FromBody] GalleryRequest model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var artist = await _artworkRepository.GetArtistByUserIdAsync(userId);
            if (artist == null)
                return BadRequest("Artist not found.");

            var gallery = await _galleryRepository.GetGalleryByIdAsync(id);
            if (gallery == null || gallery.ArtistID != artist.ArtistID)
                return NotFound("Gallery not found or unauthorized.");

            // ✅ Update only the provided fields
            if (!string.IsNullOrEmpty(model.Name))
                gallery.Name = model.Name;

            if (!string.IsNullOrEmpty(model.Description))
                gallery.Description = model.Description;

            if (!string.IsNullOrEmpty(model.Location))
                gallery.Location = model.Location;

            // Only update artworks if they are provided
            if (model.ArtworkIds != null && model.ArtworkIds.Count > 0)
            {
                if (model.ArtworkIds.Count > 20)
                    return BadRequest("A gallery can have a maximum of 20 artworks.");

                var artworks = await _artworkRepository.GetArtworksByIdsAsync(model.ArtworkIds); // ✅ Fixed method call

                if (artworks.Count != model.ArtworkIds.Count)
                    return BadRequest("One or more artworks do not exist.");

                if (artworks.Any(a => a.ArtistID != artist.ArtistID))
                    return Forbid("Some artworks do not belong to you.");

                await _galleryRepository.UpdateGalleryArtworksAsync(gallery.GalleryID, model.ArtworkIds);
            }

            //  Save the updated gallery only if any changes were made
            var isUpdated = await _galleryRepository.UpdateGalleryAsync(gallery);
            if (!isUpdated)
                return StatusCode(500, "Failed to update the gallery.");

            return Ok("Gallery updated successfully.");
        }

        [HttpGet("galleries")]
        public async Task<IActionResult> GetAllGalleries()
        {
            try
            {
                var galleries = await _galleryRepository.GetAllGalleriesAsync();
                return Ok(galleries);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Roles = "Artist")]
        [HttpDelete("delete-gallery/{id}")]
        public async Task<IActionResult> DeleteGallery(int id)
        {
            try
            {
                bool isDeleted = await _galleryRepository.DeleteGalleryAsync(id);
                if (isDeleted)
                {
                    return Ok("Gallery deleted successfully.");
                }
                return StatusCode(500, "Failed to delete the gallery.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [Authorize(Roles = "Artist")]
        //[HttpGet("my-artwork-ids")]
        //public async Task<IActionResult> GetArtworkIdsByArtist()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //        return Unauthorized("User ID not found in token.");

        //    var artist = await _artworkRepository.GetArtistByUserIdAsync(userId);
        //    if (artist == null)
        //        return BadRequest("Artist not found.");

        //    var artworkIds = await _artworkRepository
        //        .GetArtworksByArtistIdAsync(artist.ArtistID);

        //    // Return only an array of IDs
        //    return Ok(artworkIds.Select(a => a.ArtworkID).ToArray());
        //}

        [HttpGet("my-artworks")]
        public async Task<IActionResult> GetFilteredArtworksByArtist()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var artist = await _artworkRepository.GetArtistByUserIdAsync(userId);
            if (artist == null)
                return BadRequest("Artist not found.");

            // Fetch artwork IDs first
            var artworkIds = await _artworkRepository
                .GetArtworksByArtistIdAsync(artist.ArtistID);

            var artworkIdList = artworkIds.Select(a => a.ArtworkID).ToList();

            // Fetch the artworks that match the IDs
            var artworks = await _artworkRepository.GetArtworksByIdsAsync(artworkIdList);

            // Return only artworks that match the stored IDs
            var artworkDtos = artworks.Select(a => new
            {
                id = a.ArtworkID,
                title = a.Title,
                imageURL = a.ImageURL
            });

            return Ok(artworkDtos);
        }



    }
}

   

