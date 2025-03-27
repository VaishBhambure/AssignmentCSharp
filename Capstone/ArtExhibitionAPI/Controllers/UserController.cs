using ArtExhibition.Application.Model.Identity;
using ArtExhibition.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace ArtExhibitionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(request);
            return Ok(result);
        }
    }
}
