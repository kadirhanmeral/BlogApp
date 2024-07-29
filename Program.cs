using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Services.Abstract;
using BlogApp.Services.Auhentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZstdSharp.Unsafe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogContext>(options =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("mysql_connection");

    var version = new MySqlServerVersion(new Version(9, 0, 0));
    options.UseMySql(connectionString, version);
});

builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IFollowRepository, EfFollowRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/users/login");

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


SeedData.FillTestData(app);


// If the path 'posts' is entered, it throws a 'Not Found' error, otherwise both 'blogs' and 'posts' are directed to the same page.
/*app.Use(async (context, next) =>
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    if (context.Request.Path.Value.Trim('/').Equals("posts", StringComparison.OrdinalIgnoreCase))
    {

        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Not Found");
    }
    else
    {
        await next();
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
});
*/


app.MapControllerRoute(
    name: "post_details",
    pattern: "posts/details/{url}",
    defaults: new { controller = "Posts", Action = "Details" }

);

app.MapControllerRoute(
    name: "posts_by_tag",
    pattern: "posts/tag/{tag_url}",
    defaults: new { controller = "Posts", Action = "Index" }

);
app.MapControllerRoute(
    name: "user_profile",
    pattern: "profile/{username}",
    defaults: new { controller = "Users", Action = "Profile" }

);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=posts}/{action=index}/{id?}"
);


app.Run();
