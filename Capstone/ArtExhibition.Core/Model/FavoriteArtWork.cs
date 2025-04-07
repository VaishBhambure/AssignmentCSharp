using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Domain.Model
{
    public class FavoriteArtWork
    {
        
        [ForeignKey("User")]
        public string UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("Artwork")]
        public int ArtworkID { get; set; }
        public Artwork Artwork { get; set; }
    }
}
