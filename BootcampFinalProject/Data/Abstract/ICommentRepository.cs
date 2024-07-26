using BootcampFinalProject.Entity;

namespace BootcampFinalProject.Data.Abstract
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }
        void CreateComments(Comment comment);

    }
}
