using LeaveManagementApp.ViewModels;

namespace LeaveManagementApp.Services
{
    public interface IUserService
    {
        Task LogoutAsync();
        Task<bool> LoginUserAsync(LoginViewModel model);
        Task<bool> RegisterUserAsync(RegisterViewModel model);
        Task<User> GetUserByEmailAsync(string email);
        Task<IList<string>> GetUserRolesAsync(User user);
    }
}
