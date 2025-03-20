using LeaveManagementApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagementApp.Repository.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
        Task<LeaveRequest> GetLeaveRequestByIdAsync(int requestId);
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(string userId);
        Task DeleteLeaveRequestAsync(int requestId);
    }
}
