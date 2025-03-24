using LeaveManagement.Models;

namespace LeaveManagement.Repository
{
    public interface ILeaveBalanceRepository
    {
        Task<LeaveBalance> GetLeaveBalanceByUserIdAsync(int userId);
        Task UpdateLeaveBalanceAsync(LeaveBalance leaveBalance);
    }
}
