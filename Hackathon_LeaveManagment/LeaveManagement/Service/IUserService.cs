using LeaveManagement.Models;
using LeaveManagement.ViewModels;

namespace LeaveManagement.Service
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterViewModel model);
        Task<User?> AuthenticateUserAsync(string email, string password);

    }
}
