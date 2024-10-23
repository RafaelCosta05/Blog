namespace Projeto.ViewModels
{
    public class RatingVM
    {
        public int PostId { get; set; }
        public string? UserName { get; set; }
        public int Value { get; set; }
        public DateTime RatedAt { get; set; }
    }
}