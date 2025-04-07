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
    public class ArtistRepository : IArtistRepository
    {
        private readonly ArtExhibitionDbContext _context;

        public ArtistRepository(ArtExhibitionDbContext context)
        {
            _context = context;
        }

        public async Task AddArtistAsync(Artist artist)
        {
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();
        }
        public async Task<Artwork> AddArtworkAsync(Artwork artwork)
        {
            _context.Artworks.Add(artwork);
            await _context.SaveChangesAsync();
            return artwork;
        }
        public async Task<Artist> GetArtistByUserIdAsync(string userId)
        {
            return await _context.Artists.FirstOrDefaultAsync(a => a.Id == userId);
        }
        public async Task<Artwork> GetArtworkByIdAsync(int id)
        {
            return await _context.Artworks.FindAsync(id);
        }
        public async Task<List<Artwork>> GetArtworksByIdsAsync(List<int> artworkIds)
        {
            return await _context.Artworks
                .Where(a => artworkIds.Contains(a.ArtworkID))
                .ToListAsync();
        }

        public async Task UpdateArtworkAsync(Artwork artwork)
        {
            _context.Artworks.Update(artwork);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArtworkAsync(Artwork artwork)
        {
            _context.Artworks.Remove(artwork);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Artwork>> GetArtworksByArtistIdAsync(int artistId)
        {
            return await _context.Artworks
                                 .Where(a => a.ArtistID == artistId)
                                 .ToListAsync();
        }
    }
}
