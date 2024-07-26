using Microsoft.AspNetCore.Identity;

namespace BootcampFinalProject.Entity
{
    public class AppUser : IdentityUser
    {
        public string UserId
        {
            get { return this.Id; }
            set { this.Id = value; }
        }

        public string FullName { get; set; } = string.Empty;
        public string? Image { get; set; }
        public List<Post> Posts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}