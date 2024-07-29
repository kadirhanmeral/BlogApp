using BlogApp.Entity;
using BlogApp.Services.Abstract;
using BlogApp.Services.Auhentication;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {


        private static readonly IPasswordHasher _passwordHasher = new PasswordHasher();





        public static void FillTestData(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "Technology", Url = "technology", Color = TagColors.warning },
                        new Tag { Text = "AI", Url = "artificial-intelligence", Color = TagColors.secondary },
                        new Tag { Text = "LLM", Url = "large-language-model", Color = TagColors.primary },
                        new Tag { Text = "Engineering", Url = "engineering", Color = TagColors.success },
                        new Tag { Text = "Software", Url = "software", Color = TagColors.info }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {

                    context.Users.AddRange(
                        new User { UserName = "johnmartinez", Image = "admin.jpg", Name = "John", Surname = "Martinez", Password = _passwordHasher.Hash("123456"), Email = "john@gmail.com" },
                        new User { UserName = "marywilson", Image = "user.jpg", Name = "Mary", Surname = "Wilson", Password = _passwordHasher.Hash("123456"), Email = "mary@gmail.com" }
                    );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "Artificial Intelligence",
                            Content = "Artificial intelligence applications",
                            Description = "Artificial Intelligence",
                            IsActive = true,
                            Url = "artificial-intelligence",
                            PublishedOn = DateTime.Now.AddDays(-4),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "1.jpeg",
                            UserId = 1,
                            Comments = new List<Comment> {
                                 new Comment {Text="It is very helpful content.",PublishedOn=new DateTime(2024, 7, 29, 9, 45, 0),UserId=2},
                                 new Comment {Text="I've learnt lots of things, thank you!",PublishedOn=new DateTime(2024, 2, 15, 14, 30, 0),UserId=1}}



                        },
                        new Post
                        {
                            Title = "Technology",
                            Content = "Last features in technology",
                            Description = "TechnologyTechnologyTechnology",
                            IsActive = true,
                            Url = "technology",
                            PublishedOn = DateTime.Now.AddDays(-40),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "2.jpg",
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "Microsoft .Net Core",
                            Content = "Introduction to .Net Core",
                            Description = "Microsoft .Net Core",
                            IsActive = true,
                            Url = "dotnet-core",
                            PublishedOn = DateTime.Now.AddDays(-60),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "4.jpg",
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "RESTful API",
                            Content = "Introduction to RESTful api's",
                            Description = "RESTful API",

                            IsActive = true,
                            Url = "restful-api",
                            PublishedOn = DateTime.Now.AddDays(-15),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "3.png",
                            UserId = 2
                        }, new Post
                        {
                            Title = "Pointers with C language",
                            Content = "How can we use pointers?",
                            Description = "TechnologyTechnologyTechnology",
                            IsActive = true,
                            Url = "c-pointers",
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "5.png",
                            UserId = 2
                        }


                    );
                    context.SaveChanges();
                }



            }
        }
    }
}