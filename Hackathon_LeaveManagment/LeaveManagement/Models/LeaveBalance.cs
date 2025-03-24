using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.Models
{
    public class LeaveBalance
    {
        [Key]
        public int LeaveBalanceId { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; } // Navigation Property

        [Required]
        public int TotalLeaveDays { get; set; }

        [Required]
        public int RemainingLeaveDays { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    }
}
