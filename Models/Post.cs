﻿using Projeto.ViewModels;

namespace Projeto.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public DateTime CreatedAt { get; set; } =  DateTime.Now;
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsPublic { get; set; }

        public ICollection<Rating>? Ratings { get; set; }
    }
}
