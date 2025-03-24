
using LeaveManagementApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using LeaveManagementApp.Models;



namespace LeaveManagementApp.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)  // Navigation Property
                .Include(l => l.Approval) // If Approval is needed
                .ToListAsync();
        }

        public async Task<LeaveRequest> GetLeaveRequestByIdAsync(int requestId)
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.Approval)
                .FirstOrDefaultAsync(l => l.LeaveRequestId == requestId);
        }

        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveRequest));
            }

            await _context.LeaveRequests.AddAsync(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(string userId)
        {
            return await _context.LeaveRequests
                .Where(l => l.UserId == userId)
                .Include(l => l.Employee)
                .ToListAsync();
        }

        public async Task DeleteLeaveRequestAsync(int requestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(requestId);
            if (leaveRequest != null)
            {
                _context.LeaveRequests.Remove(leaveRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
