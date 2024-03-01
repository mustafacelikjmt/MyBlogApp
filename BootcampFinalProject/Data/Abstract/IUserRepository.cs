using BootcampFinalProject.Entity;

namespace BootcampFinalProject.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<AppUser> Users { get; }
        void CreateUser(AppUser user);

    }
}
