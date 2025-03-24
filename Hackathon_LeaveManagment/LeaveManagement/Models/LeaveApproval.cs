using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.Models
{
    public class LeaveApproval
    {
        public enum ApprovalStatusEnum
        {
            Pending = 0,
            Approved = 1,
            Rejected = 2
        }

        [Key]
        public int ApprovalId { get; set; }

        [Required]
        public int LeaveRequestId { get; set; }

        [ForeignKey("LeaveRequestId")]
        public LeaveRequest LeaveRequest { get; set; } // Navigation Property

        [Required]
        public int ManagerId { get; set; } // User ID of the approving Manager

        [ForeignKey("ManagerId")]
        public User Manager { get; set; } // Navigation Property

        [Required]
        public ApprovalStatusEnum ApprovalStatus { get; set; } // Uses Enum instead of string

        public DateTime ReviewedDate { get; set; } = DateTime.UtcNow;

        public string? Comments { get; set; } // Optional comments by the manager
    }
}
