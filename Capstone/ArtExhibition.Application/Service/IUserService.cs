using ArtExhibition.Application.Model;
using ArtExhibition.Application.Model.Identity;


namespace ArtExhibition.Application.Service
{
    public interface IUserService
    {
        Task<RegistrationResponse> RegisterUserAsync(RegistrationRequest request);
        Task<LoginResponse> LoginUserAsync (LoginRequest request);
        Task<UserProfile> GetUserProfileAsync(string userId);

    }
}
