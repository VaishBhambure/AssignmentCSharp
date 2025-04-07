using ArtExhibition.Application.Model;
using ArtExhibition.Domain.Interface;
using ArtExhibition.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtExhibition.Application.Service
{
    public class GalleryService : IGalleryService
    {
        private readonly IGalleryRepository _galleryRepository;
        private readonly IArtistRepository _artworkRepository;
       


        public GalleryService(IGalleryRepository galleryRepository, IArtistRepository artworkRepository)
        {
            _galleryRepository = galleryRepository;
            _artworkRepository = artworkRepository;
        }

        public async Task<Gallery> CreateGalleryAsync(Gallery gallery, List<int> artworkIds)
        {
            if (artworkIds.Count > 20)
            {
                throw new InvalidOperationException("A gallery can have a maximum of 20 artworks.");
            }

            // Validate if the artworks belong to the artist
            foreach (var artworkId in artworkIds)
            {
                var artwork = await _artworkRepository.GetArtworkByIdAsync(artworkId);
                if (artwork == null || artwork.ArtistID != gallery.ArtistID)
                {
                    throw new UnauthorizedAccessException($"Artwork with ID {artworkId} does not belong to the artist.");
                }
            }

            return await _galleryRepository.CreateGalleryAsync(gallery, artworkIds);
        }

        public async Task<Gallery> GetGalleryByIdAsync(int galleryId)
        {
            var gallery = await _galleryRepository.GetGalleryByIdAsync(galleryId);
            if (gallery == null)
            {
                throw new KeyNotFoundException($"Gallery with ID {galleryId} not found.");
            }
            return gallery;
        }

        public async Task<IEnumerable<Gallery>> GetGalleriesByArtistIdAsync(int artistId)
        {
            return await _galleryRepository.GetGalleriesByArtistIdAsync(artistId);
        }

        public async Task<bool> UpdateGalleryAsync(int galleryId, GalleryRequest request, int artistId)
        {
            var gallery = await _galleryRepository.GetGalleryByIdAsync(galleryId);
            if (gallery == null || gallery.ArtistID != artistId)
                throw new UnauthorizedAccessException("Gallery not found or unauthorized.");

            if (request.ArtworkIds.Count > 20)
                throw new InvalidOperationException("A gallery can have a maximum of 20 artworks.");

            // Update gallery details
            gallery.Name = request.Name;
            gallery.Description = request.Description;
            gallery.Location = request.Location;

            var isUpdated = await _galleryRepository.UpdateGalleryAsync(gallery);
            if (isUpdated)
                await UpdateGalleryArtworksAsync(galleryId, request.ArtworkIds, artistId);

            return isUpdated;
        }

        public async Task UpdateGalleryArtworksAsync(int galleryId, List<int> artworkIds, int artistId)
        {
            if (artworkIds.Count > 20)
                throw new InvalidOperationException("A gallery can have a maximum of 20 artworks.");

            // Validate artworks before updating
            foreach (var artworkId in artworkIds)
            {
                var artwork = await _artworkRepository.GetArtworkByIdAsync(artworkId);
                if (artwork == null || artwork.ArtistID != artistId)
                    throw new UnauthorizedAccessException($"Artwork with ID {artworkId} does not belong to you.");
            }

            await _galleryRepository.UpdateGalleryArtworksAsync(galleryId, artworkIds);
        }
        public async Task<List<Gallery>> GetAllGalleriesAsync()
        {
            var galleries = await _galleryRepository.GetAllGalleriesAsync();

            if (galleries == null || galleries.Count == 0)
                throw new KeyNotFoundException("No galleries found.");

            return galleries;
        }


        public async Task<bool> DeleteGalleryAsync(int galleryId)
        {
            var existingGallery = await _galleryRepository.GetGalleryByIdAsync(galleryId);
            if (existingGallery == null)
            {
                throw new KeyNotFoundException($"Gallery with ID {galleryId} not found.");
            }

            try
            {
                bool isDeleted = await _galleryRepository.DeleteGalleryAsync(galleryId);
                if (!isDeleted)
                {
                    throw new Exception("Failed to delete the gallery due to an unknown error.");
                }
                return true;
            }
            catch (DbUpdateException ex) 
            {
                throw new Exception($"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}");
            }
        }

    }
}
