using LeaveManagement.Models;

namespace LeaveManagement.Repository
{
    public interface ILeaveRequestRepository
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
        Task<LeaveRequest> GetLeaveRequestByIdAsync(int requestId);
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(int userId);
        Task DeleteLeaveRequestAsync(int requestId);
    }
}
