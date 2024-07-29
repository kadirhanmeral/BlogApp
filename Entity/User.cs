using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entity
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string FullName => $"{Name} {Surname}";

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Image { get; set; } = "unknown.jpg";

        public List<Post> Posts { get; set; } = new List<Post>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Follow> Following { get; set; } = new List<Follow>();
        public List<Follow> Followers { get; set; } = new List<Follow>();

    }
}