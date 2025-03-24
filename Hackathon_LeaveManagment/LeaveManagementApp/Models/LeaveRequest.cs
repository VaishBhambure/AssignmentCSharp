using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
    


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

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User Employee { get; set; }  // ✅ Uses existing 'Id' from AspNetUsers

    public LeaveTypeEnum LeaveType { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    public string Reason { get; set; }

    public DateTime AppliedDate { get; set; } = DateTime.UtcNow;



    // Navigation Property
    public LeaveApproval? Approval { get; set; } 
}
