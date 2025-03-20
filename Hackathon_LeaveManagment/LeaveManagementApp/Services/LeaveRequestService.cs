using LeaveManagementApp.Models;
using LeaveManagementApp.Repository.Interfaces;
using LeaveManagementApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagementApp.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            return await _leaveRequestRepository.GetAllLeaveRequestsAsync();
        }

        public async Task<LeaveRequest> GetLeaveRequestByIdAsync(int requestId)
        {
            return await _leaveRequestRepository.GetLeaveRequestByIdAsync(requestId);
        }

        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            await _leaveRequestRepository.AddLeaveRequestAsync(leaveRequest);
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(string userId)
        {
            return await _leaveRequestRepository.GetLeaveRequestsByUserIdAsync(userId);
        }

        public async Task DeleteLeaveRequestAsync(int requestId)
        {
            await _leaveRequestRepository.DeleteLeaveRequestAsync(requestId);
        }
    }
}
