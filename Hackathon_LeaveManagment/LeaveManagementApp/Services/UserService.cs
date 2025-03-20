//using LeaveManagementApp.Repository;
//using LeaveManagementApp.ViewModels;
//using static User;

//namespace LeaveManagementApp.Services
//{
//    public class UserService:IUserService
//    {
//        private readonly IUserRepository _userRepository;

//        public UserService(IUserRepository userRepository)
//        {
//            _userRepository = userRepository;
//        }

//        public async Task<bool> RegisterUserAsync(RegisterViewModel model)
//        {
//            var user = new User
//            {
//                Name = model.Name,
//                Email = model.Email,
//                UserName = model.Email,
//                Role = UserRole.Employee // Default Role
//            };

//            return await _userRepository.RegisterUserAsync(user, model.Password);
//        }
//    }
//}

using LeaveManagementApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LeaveManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> RegisterUserAsync(RegisterViewModel model)
        {
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email // Using Email as username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee"); // Default role
                return true;
            }
            return false;
        }

        public async Task<bool> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return false;

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
