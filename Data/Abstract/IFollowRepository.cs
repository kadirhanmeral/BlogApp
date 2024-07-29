using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IFollowRepository
    {
        IQueryable<Follow> Follows { get; }

        void Unfollow(Follow follow);
        void follow(Follow follow);
    }
}