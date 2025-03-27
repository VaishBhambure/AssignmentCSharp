using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Domain.Model
{
   public  class User: IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]

        public DateOnly BirthDate { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain only digits.")]
        public string PhoneNumber { get; set; }
        public bool IsArtist { get; set; } = false;
            // Indicates if the user is an artist
        public virtual Artist? Artist { get; set; }
        public ICollection<FavoriteArtWork> FavoriteArtWorks { get; set; }
    }

}
