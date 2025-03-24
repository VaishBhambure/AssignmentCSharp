using LeaveManagement.Exceptions;
using LeaveManagement.Models;
using LeaveManagement.Repository;
using LeaveManagement.Service;
using LeaveManagement.Exceptions;
namespace LeaveManagement.Services
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        public LeaveBalanceService(ILeaveBalanceRepository leaveBalanceRepository)
        {
            _leaveBalanceRepository = leaveBalanceRepository;
        }

        public async Task<LeaveBalance> GetLeaveBalanceByUserIdAsync(int userId)
        {
            var balance = await _leaveBalanceRepository.GetLeaveBalanceByUserIdAsync(userId);
            if (balance == null)
                throw new LeaveBalanceExceededException("Leave balance not found for this user.");

            return balance;
        }

        public async Task UpdateLeaveBalanceAsync(LeaveBalance leaveBalance)
        {
            await _leaveBalanceRepository.UpdateLeaveBalanceAsync(leaveBalance);
        }
    }
}
