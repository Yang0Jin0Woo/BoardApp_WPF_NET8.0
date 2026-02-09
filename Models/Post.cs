using System;

namespace BoardApp.Models
{
    public class Post
    {
        private static readonly TimeZoneInfo KstTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");

        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string Author { get; set; } = "";

        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }

        public DateTime CreatedAtKst =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(CreatedAtUtc, DateTimeKind.Utc), KstTimeZone);

        public DateTime UpdatedAtKst =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(UpdatedAtUtc, DateTimeKind.Utc), KstTimeZone);
    }
}
