using Microsoft.AspNetCore.Identity;

namespace BootcampFinalProject.Entity
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {
        }

        public AppRole(string roleName) : base(roleName)
        {
        }

        // Yeni rol özellikleri eklemek istersek
    }
}