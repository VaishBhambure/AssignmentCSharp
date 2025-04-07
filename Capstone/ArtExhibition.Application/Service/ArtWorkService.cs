using ArtExhibition.Application.Model;
using ArtExhibition.Domain.Interface;
using ArtExhibition.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Application.Service
{
    public class ArtWorkService : IArtWorkService
    {
        private readonly IArtWorkRepository _artworkRepository;

        public ArtWorkService(IArtWorkRepository artworkRepository)
        {
            _artworkRepository = artworkRepository;
        }

        public async Task<List<Artwork>> SearchArtworksAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Search keyword cannot be empty.");//title and description

            return await _artworkRepository.SearchArtworksAsync(keyword);
        }

        // Add Artwork to Favorites (with Validation)
        public async Task<bool> AddArtworkToFavoriteAsync(string userId, int artworkId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required.");

            if (artworkId <= 0)
                throw new ArgumentException("Invalid artwork ID.");

            bool isAdded = await _artworkRepository.AddArtworkToFavoriteAsync(userId, artworkId);

            if (!isAdded)
                throw new InvalidOperationException("Artwork is already in favorites.");

            return true;
        }

        // Remove Artwork from Favorites (with Validation)
        public async Task<bool> RemoveArtworkFromFavoriteAsync(string userId, int artworkId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required.");

            if (artworkId <= 0)
                throw new ArgumentException("Invalid artwork ID.");

            bool isRemoved = await _artworkRepository.RemoveArtworkFromFavoriteAsync(userId, artworkId);

            if (!isRemoved)
                throw new KeyNotFoundException("Artwork not found in favorites.");

            return true;
        }
        public async Task<List<Artwork>> GetUserFavoriteArtworksAsync(string userId)
        {
            return await _artworkRepository.GetUserFavoriteArtworksAsync(userId);
        }
        public async Task<List<ArtworkInfoDTO>> GetAllArtworksAsync()
        {
            var artworks = await _artworkRepository.GetAllArtworksAsync();
            var artworkInfoList = artworks.Select(a => new ArtworkInfoDTO
            {
                ArtworkID = a.ArtworkID, 
                Title = a.Title,
                Description = a.Description,
                ImageURL = a.ImageURL,
                ArtistName = a.Artist.Name
            }).ToList();

            return artworkInfoList;
        }

    }
}

