using LeaveManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using LeaveManagement.ViewModels;
using LeaveManagement.Service;

//namespace LeaveManagement.Controllers
//{
//    public class UserController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IUserService _userService;


//        public UserController(ApplicationDbContext context, IUserService userService)
//        {
//            _context = context;
//            _userService = userService;
//        }



//        [HttpGet]
//        public IActionResult Register()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Register(RegisterViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            try
//            {
//                await _userService.RegisterEmployeeAsync(model);
//                TempData["SuccessMessage"] = "Registration successful!";
//                return RedirectToAction("Login");
//            }
//            catch (Exception ex)
//            {
//                TempData["ErrorMessage"] = ex.Message;
//                return View(model);
//            }
//        }



//        // GET: User/Login
//        public IActionResult Login()
//        {
//            return View();
//        }
//        public IActionResult AccessDenied()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            if (!ModelState.IsValid)
//                return View(model);

//            var user = await _context.Users
//                                     .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

//            if (user == null)
//            {
//                ModelState.AddModelError("", "Invalid email or password");
//                return View(model);
//            }

//            // ✅ Store Role as a Claim
//            var claims = new List<Claim>
//    {
//        new Claim(ClaimTypes.Name, user.Name),
//        new Claim(ClaimTypes.Email, user.Email),
//        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
//        new Claim(ClaimTypes.Role, user.Role.ToString())  // Store role
//    };

//            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//            var authProperties = new AuthenticationProperties { IsPersistent = true };

//            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

//            // ✅ Redirect based on role
//            if (user.Role.ToString() == "Admin")
//                return RedirectToAction("AdminDashboard", "Admin");
//            else if (user.Role.ToString() == "Manager")
//                return RedirectToAction("ManagerDashboard", "Manager");
//            else if (user.Role.ToString() == "Employee")
//                return RedirectToAction("Dashboard", "EmployeeLeave");

//            return RedirectToAction("Login");
//        }



//        // GET: User/Logout
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            return RedirectToAction("Login");
//        }
//    }


//}

using LeaveManagement.Models;
using LeaveManagement.Services;
using LeaveManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LeaveManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _userService.RegisterUserAsync(model);
                TempData["SuccessMessage"] = "Registration successful!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.AuthenticateUserAsync(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            // ✅ Store Role as a Claim
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())  // Store role
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // ✅ Redirect based on role
            return user.Role.ToString() switch
            {
                "Admin" => RedirectToAction("AdminDashboard", "Admin"),
                "Manager" => RedirectToAction("ManagerDashboard", "Manager"),
                "Employee" => RedirectToAction("Dashboard", "EmployeeLeave"),
                _ => RedirectToAction("Login"),
            };
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}

