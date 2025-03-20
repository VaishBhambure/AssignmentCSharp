using LeaveManagement.Models;
using LeaveManagement.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LeaveManagement.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            Console.WriteLine("Inside RegisterUserAsync");

            if (await _userRepository.UserExistsAsync(user.Email))
            {
                Console.WriteLine("User already exists!");
                throw new Exception("Email already exists.");
            }

            // ✅ Hash password before storing
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            var createdUser = await _userRepository.AddUserAsync(user);
            if (createdUser == null)
            {
                Console.WriteLine("Failed to save user to the database.");
                throw new Exception("User registration failed.");
            }

            Console.WriteLine("User saved successfully!");
            return createdUser;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
    }
}
