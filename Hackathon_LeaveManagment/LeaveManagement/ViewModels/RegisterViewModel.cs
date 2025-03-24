using LeaveManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(12, MinimumLength = 8, ErrorMessage = "Password must be between 8-12 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]

        [NotMapped]
        
        public string ConfirmPassword { get; set; }

        [Required]
        public User.Roles Role { get; set; } = User.Roles.Employee;
    }
}
