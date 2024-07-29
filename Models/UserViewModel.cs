using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class UserViewModel
    {

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? Image { get; set; }

        public IFormFile? ImageFile { get; set; }


        public bool IsFollowing { get; set; }

        public int FollowersCount { get; set; } = 0;

        public int FollowingCount { get; set; } = 0;

        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Follow> Following { get; set; } = new List<Follow>();
        public List<Follow> Followers { get; set; } = new List<Follow>();

    }
}