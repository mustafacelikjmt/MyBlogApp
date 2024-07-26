using BootcampFinalProject.Data.Abstract;
using BootcampFinalProject.Data.Concrete.EfCore;
using BootcampFinalProject.Entity;

namespace BootcampFinalProject.Data.Concrete
{
    public class EfTagRepository : ITagRepository
    {
        private readonly BlogContext _context;

        public EfTagRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> Tags => _context.Tags;

        public void CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }
    }
}
