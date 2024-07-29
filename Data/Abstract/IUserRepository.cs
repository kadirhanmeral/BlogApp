using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void CreateUser(User user);

        void EditUser(UserViewModel post);

    }
}