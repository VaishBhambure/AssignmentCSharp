using ArtExhibition.Application.Model.Identity;
using ArtExhibition.Domain.Interface;
using ArtExhibition.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace ArtExhibition.Application.Service
{
    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly IArtistRepository _artistRepository;

        public UserService(UserManager<User> userManager, IArtistRepository artistRepository)
        {
            _userManager = userManager;
            _artistRepository = artistRepository;
        }

       
        public async Task<RegistrationResponse> RegisterUserAsync(RegistrationRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                throw new Exception($"Username '{request.UserName}' is already taken. Please choose a different one.");
            }

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                throw new Exception($"Email '{request.Email}' is already in use. Try a different email.");
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber =request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                IsArtist = request.IsArtist
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Registration failed: {errors}");
            }

            string message = "User registered successfully.";

            if (request.IsArtist)
            {
                var artist = new Artist
                {
                    Name = $"{request.FirstName} {request.LastName}",
                    BirthDate = request.BirthDate,
                    Id = user.Id,
                    PhoneNumber= request.PhoneNumber
                };

                await _artistRepository.AddArtistAsync(artist);
                message = "User registered successfully as an artist.";
            }

            return new RegistrationResponse
            {
               
                Message = message
            };
        }

        Task<AuthResponse> IUserService.Login(AuthRequest authRequest)
        {
            throw new NotImplementedException();
        }
    }

}
