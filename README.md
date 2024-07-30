# BlogApp


If you want to use MySQL, you must change the following line in appsettings.Development.json: 
```"mysql_connection":"server=localhost;port=yourPortnumber;user=root;password="yourPassword";database=blogApp"```
Replace YourPortNumber and YourPassword with your own values.


Instead of using MySQL, you can also use SQLite. To do so, change the following lines:
```
builder.Services.AddDbContext<BlogContext>
(options =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("mysql_connection");
    var version = new MySqlServerVersion(new Version(9, 0, 0));
    options.UseMySql(connectionString, version);
    
});
```
to:
```
builder.Services.AddDbContext<BlogContext>
(options =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("sql_connection");

});
```
To start the project, add migrations using the following command:
```
dotnet ef migrations add InitialCreate
```
This setup allows the project to work with either MySQL or SQLite.


Here are some key features and technologies used:

ASP.NET Core MVC & Entity Framework: Utilized EF Core for managing blog posts, comments, and user management.

Ajax & Real-Time Comments: Implemented Ajax to enable real-time commenting without refreshing the page.

HTML, CSS & Bootstrap: Designed a user-friendly and visually appealing interface using HTML, CSS, and Bootstrap.

MySQL: Chose MySQL for database management, optimizing for performance and security.

User Follow System: Integrated a feature allowing users to follow and unfollow other users.

Password Hashing: Enhanced security with hashing algorithms for storing user passwords securely.

User Profile Management: Enabled users to view and update their profiles and manage their follow lists.

Authorization & Authentication: Leveraged ASP.NET Core's built-in authentication and authorization mechanisms for secure login, registration, and role-based access control.



