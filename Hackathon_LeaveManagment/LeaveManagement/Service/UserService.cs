using LeaveManagement.Models;
using LeaveManagement.ViewModels;
using LeaveManagement.Context;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Utilities;
using LeaveManagement.Service;

namespace LeaveManagement.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(RegisterViewModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                throw new Exception("Email is already in use.");
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = PasswordHasher.HashPassword(model.Password),
                Role = model.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && PasswordHasher.VerifyPassword(password, user.Password))
            {
                return user;
            }

            return null;
        }
    }
}
