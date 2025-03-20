using LeaveManagement.Models;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task<User> AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}
