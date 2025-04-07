using ArtExhibition.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Domain.Interface
{
    public interface IArtistRepository
    {
        Task AddArtistAsync(Artist artist);
        Task<Artwork> AddArtworkAsync(Artwork artwork);
        Task<Artist> GetArtistByUserIdAsync(string userId);
        Task<Artwork> GetArtworkByIdAsync(int id);
        Task<List<Artwork>> GetArtworksByIdsAsync(List<int> artworkIds);
        Task<List<Artwork>> GetArtworksByArtistIdAsync(int artistId);
        Task UpdateArtworkAsync(Artwork artwork);
        Task DeleteArtworkAsync(Artwork artwork);

    }
}
