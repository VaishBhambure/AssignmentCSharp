using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.Models
{
    public class LeaveRequest
    {
        // Enum for Leave Type
        public enum LeaveTypeEnum
        {
            Sick,
            Casual,
            Paid,
            Unpaid
        }

        // Enum for Leave Status
        public enum LeaveStatusEnum
        {
            Pending,
            Approved,
            Rejected,
            Canceled
        }

        [Key]
        public int LeaveRequestId { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign Key for Employee

        [ForeignKey("UserId")]
        public User Employee { get; set; } // Navigation Property

        [Required]
        public LeaveTypeEnum LeaveType { get; set; } // Sick, Casual, Paid, Unpaid

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public LeaveStatusEnum Status { get; set; } // Pending, Approved, Rejected, Canceled

        [Required]
        public string Reason { get; set; }

        [Required]
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

        // One-to-One Relationship with LeaveApproval
        public LeaveApproval? LeaveApproval { get; set; }
    }
}
