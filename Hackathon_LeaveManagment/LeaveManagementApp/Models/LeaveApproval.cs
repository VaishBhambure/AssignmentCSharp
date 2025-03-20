using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LeaveApproval
{
    public enum ApprovalStatusEnum
    {
        Pending = 1,
        Approved,
        Rejected
    }
    [Key]
    public int ApprovalId { get; set; }

    [ForeignKey("LeaveRequest")]
    public int LeaveRequestId { get; set; }
    public LeaveRequest LeaveRequest { get; set; }

    [ForeignKey("User")]
    public string ManagerId { get; set; }
    public User Manager { get; set; }

    public ApprovalStatusEnum ApprovalStatus { get; set; }

    public DateTime ReviewedDate { get; set; } = DateTime.UtcNow;

    public string Comments { get; set; }
}
