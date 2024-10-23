using System.ComponentModel.DataAnnotations;

namespace Projeto.ViewModels
{
    public class EditVM
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? UserName { get; set; }

        public string? ProfilePictureUrl { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        public bool IsAdmin { get; set; }
    }
}
