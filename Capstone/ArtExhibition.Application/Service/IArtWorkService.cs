using ArtExhibition.Application.Model;
using ArtExhibition.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Application.Service
{
    public interface IArtWorkService
    {
        Task<List<Artwork>> SearchArtworksAsync(string keyword);
        Task<bool> AddArtworkToFavoriteAsync(string userId, int artworkId);
        Task<bool> RemoveArtworkFromFavoriteAsync(string userId, int artworkId);
        Task<List<Artwork>> GetUserFavoriteArtworksAsync(string userId);
        Task<List<ArtworkInfoDTO>> GetAllArtworksAsync();

    }
}
