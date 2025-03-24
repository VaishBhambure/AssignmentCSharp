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
        [StringLength(256)] 
        public string Password { get; set; } // Storing password directly (should be hashed in real use)


        [Required]
        public Roles Role { get; set; } = Roles.Employee; // Admin, Manager, Employee

        public List<LeaveRequest>? LeaveRequests { get; set; }
        public List<LeaveApproval>? Approvals { get; set; }
        public LeaveBalance? LeaveBalance { get; set; }
    }
}
