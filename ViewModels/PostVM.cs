using Projeto.Models;

namespace Projeto.ViewModels
{
    public class PostVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Slug { get; set; }
        public string? ShortDescription { get; set; }
        public double? AverageRating { get; set; }
        public bool IsPublic { get; set; }

        // Adicionando a propriedade ApplicationUser
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
