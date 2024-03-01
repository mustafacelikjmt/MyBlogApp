using System.ComponentModel.DataAnnotations;

namespace BootcampFinalProject.Entity
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Url { get; set; }
        public string? Image { get; set; }
        public DateTime PublishedOn { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public List<Tag> Tags { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}
