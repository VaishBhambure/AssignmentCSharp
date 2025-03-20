using LeaveManagement.Models;
using System.Threading.Tasks;

namespace LeaveManagement.Service
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}
