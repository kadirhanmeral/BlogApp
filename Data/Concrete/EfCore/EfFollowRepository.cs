using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public class EfFollowRepository : IFollowRepository
    {
        private BlogContext _context;

        public EfFollowRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Follow> Follows => _context.Follows;

        public void Unfollow(Follow follow)
        {
            _context.Follows.Remove(follow);
            _context.SaveChanges();

        }
        public void follow(Follow follow)
        {
            _context.Follows.Add(follow);
            _context.SaveChanges();

        }
    }
}