using ArtExhibition.Application.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Application.Interface.Identity
{
    interface IAuthService
    {
        Task<RegistrationResponse> RegisterUserAsync(RegistrationRequest request);
        Task<AuthResponse> Login(AuthRequest authRequest);
    }
}
