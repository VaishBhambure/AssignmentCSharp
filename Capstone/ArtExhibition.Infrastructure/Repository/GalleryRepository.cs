using ArtExhibition.Domain.Interface;
using ArtExhibition.Domain.Model;
using ArtExhibition.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtExhibition.Infrastructure.Repository
{
    public class GalleryRepository : IGalleryRepository
    {
        private readonly ArtExhibitionDbContext _context;

        public GalleryRepository(ArtExhibitionDbContext context)
        {
            _context = context;
        }

        public async Task<Gallery> CreateGalleryAsync(Gallery gallery, List<int> artworkIds)
        {
            _context.Galleries.Add(gallery);
            await _context.SaveChangesAsync();

            var artworkGalleries = artworkIds.Select(id => new ArtworkGallery
            {
                GalleryID = gallery.GalleryID,
                ArtworkID = id
            }).ToList();

            _context.ArtworkGalleries.AddRange(artworkGalleries);
            await _context.SaveChangesAsync();

            return gallery;
        }

        public async Task<Gallery> GetGalleryByIdAsync(int galleryId)
        {
            return await _context.Galleries
                .Include(g => g.ArtworkGalleries)
                .ThenInclude(ag => ag.Artwork)
                .FirstOrDefaultAsync(g => g.GalleryID == galleryId);
        }
        public async Task<List<Gallery>> GetAllGalleriesAsync()
        {
            return await _context.Galleries
                .Include(g => g.ArtworkGalleries) 
                .ThenInclude(ag => ag.Artwork)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gallery>> GetGalleriesByArtistIdAsync(int artistId)
        {
            return await _context.Galleries
                .Where(g => g.ArtistID == artistId)
                .Include(g => g.ArtworkGalleries)
                .ThenInclude(ag => ag.Artwork)
                .ToListAsync();
        }

        public async Task<bool> UpdateGalleryAsync(Gallery gallery)
        {
            _context.Galleries.Update(gallery);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteGalleryAsync(int galleryId)
        {
            var gallery = await _context.Galleries
      .Include(g => g.ArtworkGalleries)  // Include related artworks
      .FirstOrDefaultAsync(g => g.GalleryID == galleryId);

            if (gallery == null) return false;

            // Remove related artwork references before deleting gallery
            _context.ArtworkGalleries.RemoveRange(gallery.ArtworkGalleries);

            _context.Galleries.Remove(gallery);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task UpdateGalleryArtworksAsync(int galleryId, List<int> artworkIds)
        {
            var gallery = await _context.Galleries
                .Include(g => g.ArtworkGalleries)
                .FirstOrDefaultAsync(g => g.GalleryID == galleryId);

            if (gallery == null)
                throw new KeyNotFoundException("Gallery not found.");

            // Remove existing artwork associations
            _context.ArtworkGalleries.RemoveRange(gallery.ArtworkGalleries);

            // Add new artwork associations
            var newArtworkGalleries = artworkIds.Select(artworkId => new ArtworkGallery
            {
                GalleryID = galleryId,
                ArtworkID = artworkId
            }).ToList();

            await _context.ArtworkGalleries.AddRangeAsync(newArtworkGalleries);
            await _context.SaveChangesAsync();
        }

    }
}
