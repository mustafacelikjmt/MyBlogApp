using BootcampFinalProject.Entity;

namespace BootcampFinalProject.Data.Abstract
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags { get; }
        void CreateTag(Tag tag);

    }
}
