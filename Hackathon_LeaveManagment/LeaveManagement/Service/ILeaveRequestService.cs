using LeaveManagement.Models;

namespace LeaveManagement.Services
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
        Task<LeaveRequest> GetLeaveRequestByIdAsync(int requestId);
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(int userId);
        Task DeleteLeaveRequestAsync(int requestId);
    }
}
