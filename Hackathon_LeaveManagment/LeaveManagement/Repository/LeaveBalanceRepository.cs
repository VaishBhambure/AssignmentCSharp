using LeaveManagement.Context;
using LeaveManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveBalanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveBalance> GetLeaveBalanceByUserIdAsync(int userId)
        {
            return await _context.LeaveBalances.FirstOrDefaultAsync(lb => lb.UserId == userId);
        }

        public async Task UpdateLeaveBalanceAsync(LeaveBalance leaveBalance)
        {
            _context.LeaveBalances.Update(leaveBalance);
            await _context.SaveChangesAsync();
        }
    }
}
