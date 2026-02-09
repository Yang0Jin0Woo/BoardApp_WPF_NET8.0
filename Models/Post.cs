using System;

namespace BoardApp.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string Author { get; set; } = "";

        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset UpdatedAtUtc { get; set; }
    }
}

