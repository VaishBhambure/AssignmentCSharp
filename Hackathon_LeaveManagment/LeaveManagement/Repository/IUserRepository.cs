using LeaveManagement.Models;

namespace LeaveManagement.Repository
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}
