﻿namespace PiensaPeru.API.Domain.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
    }
}
