namespace Projeto.ViewModels
{
    public class BlogPostVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? shortDescription { get; set; }
        public string? Description { get; set; }

        public double? AverageRating { get; set; }

        // Rating do usuário logado (caso ele tenha avaliado)
        public int? UserRating { get; set; }
    }
}
