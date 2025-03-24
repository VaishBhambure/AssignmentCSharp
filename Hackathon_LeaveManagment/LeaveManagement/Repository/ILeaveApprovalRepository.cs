using LeaveManagement.Models;

namespace LeaveManagement.Repository
{
    public interface ILeaveApprovalRepository
    {
        Task<IEnumerable<LeaveApproval>> GetAllLeaveApprovalsAsync();
        Task<LeaveApproval> GetLeaveApprovalByIdAsync(int approvalId);
        Task AddLeaveApprovalAsync(LeaveApproval approval);
        Task<IEnumerable<LeaveApproval>> GetApprovalsByManagerIdAsync(int managerId);
        Task<bool> UpdateLeaveApprovalAsync(LeaveApproval leaveApproval);
    }

}
