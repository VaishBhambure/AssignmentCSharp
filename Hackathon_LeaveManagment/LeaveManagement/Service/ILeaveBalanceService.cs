using LeaveManagement.Models;

namespace LeaveManagement.Service
{
    public interface ILeaveBalanceService
    {
        Task<LeaveBalance> GetLeaveBalanceByUserIdAsync(int userId);
        Task UpdateLeaveBalanceAsync(LeaveBalance leaveBalance);
    }
}
