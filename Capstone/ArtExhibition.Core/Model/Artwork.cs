using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArtExhibition.Domain.Model
{
    public class Artwork
    {
        [Key]
        public int ArtworkID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public string ImageURL { get; set; }

        [ForeignKey("Artist")]
        public int ArtistID { get; set; }
        public Artist Artist { get; set; }

        public ICollection<ArtworkGallery> ArtworkGalleries { get; set; }
        public ICollection<FavoriteArtWork> FavoritedBy { get; set; }
    }
     

}
