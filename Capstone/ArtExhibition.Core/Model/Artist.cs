using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Domain.Model
{
    public class Artist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArtistID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]

        public DateOnly BirthDate { get; set; }
        
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain only digits.")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Id { get; set; }

        [ForeignKey ("Id") ]
        public virtual User User { get; set; }

        public ICollection<Gallery> Galleries { get; set; }
        public ICollection<Artwork> Artworks { get; set; }
    }
}
