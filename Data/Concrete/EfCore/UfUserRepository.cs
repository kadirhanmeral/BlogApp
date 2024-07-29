using BlogApp.Data.Concrete.EfCore;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfUserRepository : IUserRepository
    {
        private readonly BlogContext _context;

        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void EditUser(UserViewModel model)
        {
            var user = _context.Users.FirstOrDefault(i => i.UserId == model.UserId);

            if (user != null)
            {
                user.Image = model.Image;
                _context.SaveChanges();
            }

        }
    }
}
