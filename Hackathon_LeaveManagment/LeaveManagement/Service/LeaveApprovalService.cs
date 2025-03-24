



using LeaveManagement.Context;
using LeaveManagement.Exceptions;
using LeaveManagement.Models;
using LeaveManagement.Repository;
using LeaveManagement.Service;
using Microsoft.EntityFrameworkCore;

//namespace LeaveManagement.Services
//{
//    public class LeaveApprovalService : ILeaveApprovalService
//    {
//        private readonly ILeaveApprovalRepository _leaveApprovalRepository;
//        private readonly ILeaveRequestRepository _leaveRequestRepository;
//        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
//        private readonly ApplicationDbContext _context;

//        public LeaveApprovalService(
//            ILeaveApprovalRepository leaveApprovalRepository,
//            ILeaveRequestRepository leaveRequestRepository,
//            ILeaveBalanceRepository leaveBalanceRepository,
//            ApplicationDbContext context)
//        {
//            _leaveApprovalRepository = leaveApprovalRepository;
//            _leaveRequestRepository = leaveRequestRepository;
//            _leaveBalanceRepository = leaveBalanceRepository;
//            _context = context;
//        }

//        public async Task<IEnumerable<LeaveApproval>> GetAllLeaveApprovalsAsync()
//        {
//            return await _context.LeaveApprovals
//      .Include(a => a.LeaveRequest) // Get Leave Request details (includes Employee reason)
//      .Include(a => a.Manager) // Get Manager details
//      .ToListAsync();
//        }

//        public async Task<LeaveApproval> GetLeaveApprovalByIdAsync(int approvalId)
//        {
//            var approval = await _leaveApprovalRepository.GetLeaveApprovalByIdAsync(approvalId);
//            if (approval == null)
//                throw new LeaveRequestNotFoundException("Leave approval not found.");
//            return approval;
//        }

//        public async Task AddLeaveApprovalAsync(LeaveApproval approval)
//        {
//            _context.LeaveApprovals.Add(approval);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<bool> ApproveLeaveRequestAsync(int leaveRequestId, int managerId, bool isApproved, string managerComment)
//        {
//            // ✅ Fetch leave request
//            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(leaveRequestId);
//            if (leaveRequest == null || leaveRequest.Status != LeaveRequest.LeaveStatusEnum.Pending)
//                return false;

//            if (isApproved)
//            {
//                int leaveDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

//                var leaveBalance = await _leaveBalanceRepository.GetLeaveBalanceByUserIdAsync(leaveRequest.UserId);
//                if (leaveBalance == null || leaveBalance.RemainingLeaveDays < leaveDays)
//                    return false;  // Not enough leave balance

//                // Deduct leave days
//                leaveBalance.RemainingLeaveDays -= leaveDays;
//                leaveBalance.TotalLeaveDays += leaveDays;
//                await _leaveBalanceRepository.UpdateLeaveBalanceAsync(leaveBalance);
//            }

//            // ✅ Update leave request status
//            leaveRequest.Status = isApproved ? LeaveRequest.LeaveStatusEnum.Approved : LeaveRequest.LeaveStatusEnum.Rejected;
//            await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);

//            // ✅ Save approval details
//            var leaveApproval = new LeaveApproval
//            {
//                LeaveRequestId = leaveRequestId,
//                ManagerId = managerId,
//                ApprovalStatus = isApproved ? LeaveApproval.ApprovalStatusEnum.Approved : LeaveApproval.ApprovalStatusEnum.Rejected,
//                ReviewedDate = DateTime.UtcNow,
//                Comments = managerComment
//            };

//            await _leaveApprovalRepository.AddLeaveApprovalAsync(leaveApproval);

//            // ✅ Update leave approval entry
//            return await _leaveApprovalRepository.UpdateLeaveApprovalAsync(leaveApproval);
//        }


//        public async Task<IEnumerable<LeaveApproval>> GetApprovalsByManagerIdAsync(int managerId)
//        {
//            return await _leaveApprovalRepository.GetApprovalsByManagerIdAsync(managerId);
//        }


//    }
//}





using LeaveManagement.Exceptions;

public class LeaveApprovalService : ILeaveApprovalService
{
    private readonly ILeaveApprovalRepository _leaveApprovalRepository;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveBalanceRepository _leaveBalanceRepository;
    private readonly ApplicationDbContext _context;

    public LeaveApprovalService(
        ILeaveApprovalRepository leaveApprovalRepository,
        ILeaveRequestRepository leaveRequestRepository,
        ILeaveBalanceRepository leaveBalanceRepository,
        ApplicationDbContext context)
    {
        _leaveApprovalRepository = leaveApprovalRepository;
        _leaveRequestRepository = leaveRequestRepository;
        _leaveBalanceRepository = leaveBalanceRepository;
        _context = context;
    }

    public async Task<IEnumerable<LeaveApproval>> GetAllLeaveApprovalsAsync()
    {
        return await _context.LeaveApprovals
            .Include(a => a.LeaveRequest)
            .Include(a => a.Manager)
            .ToListAsync();
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
        _context.LeaveApprovals.Add(approval);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ApproveLeaveRequestAsync(int leaveRequestId, int managerId, bool isApproved, string managerComment)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(leaveRequestId);
                if (leaveRequest == null || leaveRequest.Status != LeaveRequest.LeaveStatusEnum.Pending)
                    return false;

                if (isApproved)
                {
                    int leaveDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

                    var leaveBalance = await _leaveBalanceRepository.GetLeaveBalanceByUserIdAsync(leaveRequest.UserId);
                    if (leaveBalance == null || leaveBalance.RemainingLeaveDays < leaveDays)
                        return false;  // Not enough leave balance

                    // Deduct leave days
                    leaveBalance.RemainingLeaveDays -= leaveDays;
                    leaveBalance.TotalLeaveDays += leaveDays;
                    await _leaveBalanceRepository.UpdateLeaveBalanceAsync(leaveBalance);
                }

                // Update leave request status
                leaveRequest.Status = isApproved ? LeaveRequest.LeaveStatusEnum.Approved : LeaveRequest.LeaveStatusEnum.Rejected;
                await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);

                // Save approval details
                var leaveApproval = new LeaveApproval
                {
                    LeaveRequestId = leaveRequestId,
                    ManagerId = managerId,
                    ApprovalStatus = isApproved ? LeaveApproval.ApprovalStatusEnum.Approved : LeaveApproval.ApprovalStatusEnum.Rejected,
                    ReviewedDate = DateTime.UtcNow,
                    Comments = managerComment
                };

                await _leaveApprovalRepository.AddLeaveApprovalAsync(leaveApproval);

                // Commit transaction
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Rollback transaction in case of error
                await transaction.RollbackAsync();
                // Log the exception (consider using a logging framework like Serilog or NLog)
                Console.WriteLine($"Error approving leave request: {ex.Message}");
                return false;
            }
        }
    }

    public async Task<IEnumerable<LeaveApproval>> GetApprovalsByManagerIdAsync(int managerId)
    {
        return await _leaveApprovalRepository.GetApprovalsByManagerIdAsync(managerId);
    }
}