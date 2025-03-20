namespace LeaveManagementApp.Services
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
        Task<LeaveRequest> GetLeaveRequestByIdAsync(int requestId);
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(string userId);
        Task DeleteLeaveRequestAsync(int requestId);
    }
}
