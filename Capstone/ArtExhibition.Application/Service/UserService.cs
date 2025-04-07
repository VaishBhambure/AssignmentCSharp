using ArtExhibition.Application.Model.Identity;
using ArtExhibition.Domain.Interface;
using ArtExhibition.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ArtExhibition.Application.Exceptions;
using Microsoft.Extensions.Options;
using ArtExhibition.Application.Model;


namespace ArtExhibition.Application.Service
{
    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly IArtistRepository _artistRepository;
        private readonly IConfiguration _configuration; // Inject IConfiguration
        readonly SignInManager<User> _signInManger;
        readonly JwtSettings _jwtSettings;
        private readonly IArtWorkRepository _artworkRepository;


        public UserService(UserManager<User> userManager, IArtistRepository artistRepository, IConfiguration configuration, IOptionsSnapshot<JwtSettings> jwtSettings, SignInManager<User> signInManager, IArtWorkRepository artworkRepository)
        {
            _userManager = userManager;
            _artistRepository = artistRepository;
            _configuration = configuration;
            _jwtSettings = jwtSettings.Value;
            _signInManger = signInManager;
            _artworkRepository = artworkRepository;
        }



        public async Task<LoginResponse> LoginUserAsync(LoginRequest authRequest)
        {
            var user = await _userManager.FindByEmailAsync(authRequest.Email);
            if (user == null)
            {
                throw new NotFoundException($"user with Email{authRequest.Email} not exists");
            }
            var userPassword = await _signInManger.CheckPasswordSignInAsync(user, authRequest.Password, false);
            if (userPassword.Succeeded == false)
            {
                throw new BadRequestException($" password Credentials are wrong");
            }
            var roles = await _userManager.GetRolesAsync(user);
            JwtSecurityToken jwtSecurityToken = await GenerateToken(user, roles);
            var response = new LoginResponse
            {
                
                UserName = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Roles= roles.ToList()


            };
            // string userJwtSecurityTokenHandler = JsonConvert.SerializeObject(new { Token = response.Token, Name = response.UserName });
            return response;
        }

        private async Task<JwtSecurityToken> GenerateToken(User user, IList<string> roles)
        {
            var authClaims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("username", user.UserName)
    };

            // Add roles as claims
            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }


        public async Task<RegistrationResponse> RegisterUserAsync(RegistrationRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                throw new Exception($"Username '{request.UserName}' is already taken.");
            }

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                throw new Exception($"Email '{request.Email}' is already in use.");
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                IsArtist = request.IsArtist == null ? false : request.IsArtist
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception($"Registration failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");

            string message = "User registered successfully.";

            if (request.IsArtist)
            {
                await _userManager.AddToRoleAsync(user, "Artist"); // Assign Artist role

                var artist = new Artist
                {
                    Name = $"{request.FirstName} {request.LastName}",
                    BirthDate = request.BirthDate,
                    Id = user.Id,
                    PhoneNumber = request.PhoneNumber
                };

                await _artistRepository.AddArtistAsync(artist);
                message = "User registered successfully as an artist.";
            }

            return new RegistrationResponse { Message = message };
        }
        public async Task<UserProfile> GetUserProfileAsync(string userId)
        {
            // Fetch the user from the database using the provided userId
            var user = await _userManager.FindByIdAsync(userId); // Corrected method

            if (user == null)
                return null;

            // Fetch the user's favorite artworks
            var favoriteArtworks = await _artworkRepository.GetUserFavoriteArtworksAsync(userId);

            // Return the user profile with the required fields
            return new UserProfile
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                Email = user.Email,
                FavoriteArtworks = favoriteArtworks
            };
        }


    }


}

