using BootcampFinalProject.Data.Abstract;
using BootcampFinalProject.Data.Concrete.EfCore;
using BootcampFinalProject.Entity;

namespace BootcampFinalProject.Data.Concrete
{
    public class EfUserRepository : IUserRepository
    {
        private readonly BlogContext _context;

        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<AppUser> Users => _context.Users;

        public void CreateUser(AppUser user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}