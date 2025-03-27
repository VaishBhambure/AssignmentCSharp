using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Domain.Model
{
    public class Gallery
    {
        [Key]
        public int GalleryID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        [ForeignKey("Artist")]
        public int ArtistID { get; set; }
        public Artist Artist { get; set; }

        public ICollection<ArtworkGallery> ArtworkGalleries { get; set; }
    }
}
