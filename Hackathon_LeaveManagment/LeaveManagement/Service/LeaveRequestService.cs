using LeaveManagement.Context;
using LeaveManagement.Exceptions;
using LeaveManagement.Models;
using LeaveManagement.Repository;
using LeaveManagement.Service;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ApplicationDbContext _context;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, ApplicationDbContext context)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _context = context;
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            return await _context.LeaveRequests
                          .Include(lr => lr.Employee) // Ensure Employee data is loaded
                          .ToListAsync();
        }

        public async Task<LeaveRequest> GetLeaveRequestByIdAsync(int requestId)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(requestId);
            if (leaveRequest == null)
                throw new LeaveRequestNotFoundException("Leave request not found.");

            return leaveRequest;
        }

        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            var user = await _context.Users.FindAsync(leaveRequest.UserId);
            if (user == null)
                throw new Exception("User not found.");

            if (user.Role != User.Roles.Employee)
                throw new Exception("Only employees can apply for leave.");

            var leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(lb => lb.UserId == leaveRequest.UserId);
            if (leaveBalance == null)
                throw new Exception("Leave balance record not found.");

            int leaveDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;
            if (leaveBalance.RemainingLeaveDays < leaveDays)
                throw new LeaveBalanceExceededException("Not enough leave balance.");

            leaveRequest.AppliedDate = DateTime.UtcNow;
            leaveRequest.Status = LeaveRequest.LeaveStatusEnum.Pending;

            await _leaveRequestRepository.AddLeaveRequestAsync(leaveRequest);
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(int userId)
        {
            return await _leaveRequestRepository.GetLeaveRequestsByUserIdAsync(userId);
        }

        public async Task DeleteLeaveRequestAsync(int requestId)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(requestId);
            if (leaveRequest == null)
                throw new LeaveRequestNotFoundException("Leave request not found.");

            if (leaveRequest.Status == LeaveRequest.LeaveStatusEnum.Approved)
                throw new Exception("Cannot delete an approved leave request.");

            await _leaveRequestRepository.DeleteLeaveRequestAsync(requestId);
        }
        public async Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }
    }
}
