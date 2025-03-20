using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.Models
{
    public class User
    {

        public enum Roles
        {
            Admin=1,
            Manager,
            Employee
        }
        [Key]
        public int UserId { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // Store hashed password

        [NotMapped] // Not stored in DB
        [Required]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Password must be between 8-12 characters.")]
        public string Password { get; set; } // Used only for registration

        [NotMapped] // Not stored in DB
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public Roles Role { get; set; } = Roles.Employee; // Admin, Manager, Employee

        public List<LeaveRequest>? LeaveRequests { get; set; }
        public List<LeaveApproval>? Approvals { get; set; }
        public LeaveBalance? LeaveBalance { get; set; }
    }
}
