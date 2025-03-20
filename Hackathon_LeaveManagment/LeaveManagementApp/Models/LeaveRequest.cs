using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LeaveRequest
{
    public enum LeaveTypeEnum
    {
        Sick = 1,
        Paid,
        Unpaid
    }
    public enum LeaveStatus
    {
        Pending = 1,
        Approved,
        Rejected
    }
    [Key]
    public int LeaveRequestId { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } 
    public User Employee { get; set; }

    public LeaveTypeEnum LeaveType { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    public string Reason { get; set; }

    public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public LeaveApproval Approval { get; set; }
}
