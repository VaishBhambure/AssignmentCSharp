



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
                   .ThenInclude(lr => lr.Employee)  // Include employee details
               .Include(a => a.Manager)           // Include manager details
               .ToListAsync();
    }

    public async Task<LeaveApproval> GetLeaveApprovalByIdAsync(int approvalId)
    {
        var approval = await _context.LeaveApprovals
              .Include(a => a.LeaveRequest)
                  .ThenInclude(lr => lr.Employee)
              .Include(a => a.Manager)
              .FirstOrDefaultAsync(a => a.ManagerId == approvalId);

        if (approval == null)
            throw new LeaveRequestNotFoundException($"Leave approval with ID {approvalId} not found.");

        return approval;

    }

    public async Task AddLeaveApprovalAsync(LeaveApproval approval)
    {
        await _leaveApprovalRepository.AddLeaveApprovalAsync(approval);
    }
    public async Task<bool> ApproveLeaveRequestAsync(int leaveRequestId, int managerId, LeaveApproval.ApprovalStatusEnum status, string managerComment)
    {
        // Retrieve the leave request
        var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
        if (leaveRequest == null)
            throw new LeaveRequestNotFoundException(leaveRequestId);

        //// If approved, check and update leave balance logic
        //if (status == LeaveApproval.ApprovalStatusEnum.Approved)
        //{
        //    var leaveBalance = await _context.LeaveBalances
        //                                     .FirstOrDefaultAsync(lb => lb.UserId == leaveRequest.UserId);
        //    if (leaveBalance == null)
        //        throw new Exception($"Leave balance record not found for user {leaveRequest.UserId}.");

        //    if (leaveBalance.RemainingLeaveDays < leaveRequest.NumberOfDays)
        //        throw new Exception($"Insufficient leave balance for user {leaveRequest.UserId}.");

        //    leaveBalance.RemainingLeaveDays -= leaveRequest.NumberOfDays;
        //    leaveBalance.LastUpdate = DateTime.UtcNow;
        //}
        if (status == LeaveApproval.ApprovalStatusEnum.Approved)
        {
            var leaveBalance = await _context.LeaveBalances
                                             .FirstOrDefaultAsync(lb => lb.UserId == leaveRequest.UserId);
            if (leaveBalance == null)
                throw new Exception($"Leave balance record not found for user {leaveRequest.UserId}.");

            if (leaveBalance.RemainingLeaveDays < leaveRequest.NumberOfDays)
                throw new Exception($"Insufficient leave balance for user {leaveRequest.UserId}.");

            leaveBalance.RemainingLeaveDays -= leaveRequest.NumberOfDays;
            leaveBalance.LastUpdate = DateTime.UtcNow;

            _context.LeaveBalances.Update(leaveBalance); // ✅ Explicitly update balance
        }


        // Create a new leave approval record
        var leaveApproval = new LeaveApproval
        {
            LeaveRequestId = leaveRequestId,
            ManagerId = managerId,
            ApprovalStatus = status,  // Use enum directly
            Comments = managerComment,
            ReviewedDate = DateTime.UtcNow
        };

        await _context.LeaveApprovals.AddAsync(leaveApproval);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine("Error saving changes in ApproveLeaveRequestAsync:");
            Console.WriteLine("Message: " + dbEx.Message);
            Console.WriteLine("Inner Exception: " + dbEx.InnerException?.Message);
            throw;
        }

        return true;
    }




    public async Task<IEnumerable<LeaveApproval>> GetApprovalsByManagerIdAsync(int managerId)
    {
        return await _context.LeaveApprovals
        .Where(a => a.ManagerId == managerId) // Ensure managerId is being used
        .Include(a => a.LeaveRequest)
            .ThenInclude(lr => lr.Employee)
        .Include(a => a.Manager)    
        .ToListAsync();
    }
}