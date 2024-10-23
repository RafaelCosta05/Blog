using Projeto.Models;
using X.PagedList;

namespace Projeto.ViewModels
{
    public class HomeVM
    {
        // Propriedades para os posts e o dicionário de anos/mês
        public IEnumerable<PostVM> Posts { get; set; } = new List<PostVM>();
        public Dictionary<int, List<int>> AvailableYears { get; set; } = new Dictionary<int, List<int>>();

        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        public List<PostVM>? PostsVM { get; set; }
    }
}
