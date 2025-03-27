

namespace ArtExhibition.Application.Model.Identity
{
    class AuthResponse
    {
        // public string Id { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public string Message { get; set; }
    }
}
