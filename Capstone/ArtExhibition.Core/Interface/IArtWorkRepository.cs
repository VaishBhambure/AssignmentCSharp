using ArtExhibition.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Domain.Interface
{
    public interface IArtWorkRepository
    {
        Task<List<Artwork>> SearchArtworksAsync(string keyword);
        Task<bool> AddArtworkToFavoriteAsync(string userId, int artworkId);
        Task<bool> RemoveArtworkFromFavoriteAsync(string userId, int artworkId);
        Task<List<Artwork>> GetUserFavoriteArtworksAsync(string userId);

      
        Task<List<Artwork>> GetAllArtworksAsync();
    
}
}
