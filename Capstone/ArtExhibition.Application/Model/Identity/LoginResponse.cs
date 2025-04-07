

namespace ArtExhibition.Application.Model.Identity
{
    public class LoginResponse
    {
       
        public string UserName { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
