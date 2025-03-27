using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Domain.Model
{
    public class ArtworkGallery
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Artwork")]
        public int ArtworkID { get; set; }
        public Artwork Artwork { get; set; }

        [ForeignKey("Gallery")]
        public int GalleryID { get; set; }
        public Gallery Gallery { get; set; }
    }
}
