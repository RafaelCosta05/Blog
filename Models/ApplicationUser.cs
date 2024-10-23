using Microsoft.AspNetCore.Identity;

namespace Projeto.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public List<Post>? Posts { get; set; }
    }
}
