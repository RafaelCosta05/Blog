using System.ComponentModel.DataAnnotations;

namespace Projeto.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post? Post { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }

        public DateTime RatedAt { get; set; } = DateTime.Now;
    }
}
