using LeaveManagement.Context;
using LeaveManagement.Exceptions;
using LeaveManagement.Models;
using LeaveManagement.Repository;
using LeaveManagement.Service;
using Microsoft.EntityFrameworkCore;
using static LeaveManagement.Models.LeaveApproval;

namespace LeaveManagement.Services
{
    public class LeaveApprovalService : ILeaveApprovalService
    {
        private readonly ILeaveApprovalRepository _leaveApprovalRepository;
        private readonly ApplicationDbContext _context;

        public LeaveApprovalService(ILeaveApprovalRepository leaveApprovalRepository, ApplicationDbContext context)
        {
            _leaveApprovalRepository = leaveApprovalRepository;
            _context = context;
        }

        public async Task<IEnumerable<LeaveApproval>> GetAllLeaveApprovalsAsync()
        {
            return await _leaveApprovalRepository.GetAllLeaveApprovalsAsync();
        }

        public async Task<LeaveApproval> GetLeaveApprovalByIdAsync(int approvalId)
        {
            var approval = await _leaveApprovalRepository.GetLeaveApprovalByIdAsync(approvalId);
            if (approval == null)
                throw new LeaveRequestNotFoundException("Leave approval not found.");

            return approval;
        }

        public async Task AddLeaveApprovalAsync(LeaveApproval approval)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(approval.LeaveRequestId);
            if (leaveRequest == null)
                throw new LeaveRequestNotFoundException("Leave request not found.");

            approval.ReviewedDate = DateTime.UtcNow;
            await _leaveApprovalRepository.AddLeaveApprovalAsync(approval);

            // ✅ Update leave balance if request is approved
            if (approval.ApprovalStatus == ApprovalStatusEnum.Approved)

            {
                var employeeBalance = await _context.LeaveBalances.FirstOrDefaultAsync(lb => lb.UserId == leaveRequest.UserId);
                if (employeeBalance != null)
                {
                    int leaveDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;
                    if (employeeBalance.RemainingLeaveDays >= leaveDays)
                    {
                        employeeBalance.RemainingLeaveDays -= leaveDays;
                        employeeBalance.LastUpdate = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new LeaveBalanceExceededException("Not enough leave balance.");
                    }
                }
            }
        }

        public async Task<IEnumerable<LeaveApproval>> GetApprovalsByManagerIdAsync(int managerId)
        {
            return await _leaveApprovalRepository.GetApprovalsByManagerIdAsync(managerId);
        }
    }
}
