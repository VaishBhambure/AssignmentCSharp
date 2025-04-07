using ArtExhibition.Application.Model;
using ArtExhibition.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Application.Service
{
    public interface IGalleryService
    {
        Task<Gallery> CreateGalleryAsync(Gallery gallery, List<int> artworkIds);
        Task<Gallery> GetGalleryByIdAsync(int galleryId);
        Task<IEnumerable<Gallery>> GetGalleriesByArtistIdAsync(int artistId);
        Task<List<Gallery>> GetAllGalleriesAsync();
        Task<bool> UpdateGalleryAsync(int galleryId, GalleryRequest request, int artistId);

        Task UpdateGalleryArtworksAsync(int galleryId, List<int> artworkIds, int artistId);
        Task<bool> DeleteGalleryAsync(int galleryId);
    }
}
