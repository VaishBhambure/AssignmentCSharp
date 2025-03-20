using LeaveManagement.Models;
using LeaveManagement.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeaveManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserController(IUserService userService)
        {
            _userService = userService;
            _passwordHasher = new PasswordHasher<User>();
        }

        
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // ✅ Hash the Password before storing
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            // ❌ Clear ConfirmPassword to enhance security
            user.ConfirmPassword = null;

            var createdUser = await _userService.RegisterUserAsync(user);
            if (createdUser == null)
            {
                return BadRequest(new { message = "User registration failed!" });
            }

            TempData["SuccessMessage"] = "User registered successfully!";
            return RedirectToAction("Login");
        }

    }
}

