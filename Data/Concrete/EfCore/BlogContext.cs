using Microsoft.EntityFrameworkCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EfCore
{
    public class BlogContext : DbContext

    {

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        public DbSet<Post> Posts => Set<Post>();

        public DbSet<Comment> Comments => Set<Comment>();

        public DbSet<Tag> Tags => Set<Tag>();

        public DbSet<User> Users => Set<User>();

        public DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follow>()
                .HasKey(f => new { f.FollowerId, f.FolloweeId });

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Followee)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }

}
