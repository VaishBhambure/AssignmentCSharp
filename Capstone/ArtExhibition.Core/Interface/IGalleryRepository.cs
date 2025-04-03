using ArtExhibition.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Domain.Interface
{
    public interface IGalleryRepository
    {
        Task<Gallery> CreateGalleryAsync(Gallery gallery, List<int> artworkIds);

        Task<IEnumerable<Gallery>> GetGalleriesByArtistIdAsync(int artistId);
        Task<Gallery> GetGalleryByIdAsync(int galleryId);
        Task<List<Gallery>> GetAllGalleriesAsync();
        Task<bool> UpdateGalleryAsync(Gallery gallery);
        Task UpdateGalleryArtworksAsync(int galleryId, List<int> artworkIds);
        Task<bool> DeleteGalleryAsync(int galleryId);
    }
}
