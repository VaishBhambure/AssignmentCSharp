using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LeaveBalance
{
    [Key]
    public int BalanceId { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }
    public User Employee { get; set; }

    public int TotalLeaveDays { get; set; }
    public int RemainingLeaveDays { get; set; }

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
