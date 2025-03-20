namespace LeaveManagementApp.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> RegisterUserAsync(User user, string password);
    }
}
