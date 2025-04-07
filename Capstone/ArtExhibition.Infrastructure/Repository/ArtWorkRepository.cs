using ArtExhibition.Domain.Interface;
using ArtExhibition.Domain.Model;
using ArtExhibition.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Infrastructure.Repository
{
    public class ArtWorkRepository: IArtWorkRepository
    {
        private readonly ArtExhibitionDbContext _context;

        public ArtWorkRepository(ArtExhibitionDbContext context)
        {
            _context = context;
        }

        // Search Artworks by keyword (Title or Description)

        public async Task<List<Artwork>> SearchArtworksAsync(string keyword)
        {
            return await _context.Artworks
                .Where(a => a.Title.Contains(keyword) || a.Description.Contains(keyword))
                .ToListAsync();
        }

        // Add Artwork to Favorites
        public async Task<bool> AddArtworkToFavoriteAsync(string userId, int artworkId)
        {
            //var existingFavorite = await _context.FavoriteArtWorks
            //    .AnyAsync(f => f.UserID == userId && f.ArtworkID == artworkId);

            //if (existingFavorite) return false; // Prevent duplicate favorite

            //var favorite = new FavoriteArtWork
            //{
            //    UserID = userId,
            //    ArtworkID = artworkId
            //};

            //_context.FavoriteArtWorks.Add(favorite);
            //return await _context.SaveChangesAsync() > 0;
            var alreadyExists = await _context.FavoriteArtWorks
        .AnyAsync(f => f.UserID == userId && f.ArtworkID == artworkId);

            if (alreadyExists)
                return false;

            // Add if not exists
            var favorite = new FavoriteArtWork
            {
                UserID = userId,
                ArtworkID = artworkId
            };

            _context.FavoriteArtWorks.Add(favorite);
            await _context.SaveChangesAsync();

            return true;
        }

        // Remove Artwork from Favorites
        public async Task<bool> RemoveArtworkFromFavoriteAsync(string userId, int artworkId)
        {
            var favorite = await _context.FavoriteArtWorks
                .FirstOrDefaultAsync(f => f.UserID == userId && f.ArtworkID == artworkId);

            if (favorite == null) return false;

            _context.FavoriteArtWorks.Remove(favorite);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<Artwork>> GetUserFavoriteArtworksAsync(string userId)
        {
            return await _context.FavoriteArtWorks
                .Where(f => f.UserID == userId)
                .Select(f => f.Artwork)
                .ToListAsync();
        }
        public async Task<List<Artwork>> GetAllArtworksAsync()
        {
            // Include Artist to get the artist's details (e.g., Username)
            return await _context.Artworks
                .Include(a => a.Artist)  // Ensures that the Artist is included in the result
                .ToListAsync();  // Retrieves the artworks asynchronously
        }

   

    }
}
