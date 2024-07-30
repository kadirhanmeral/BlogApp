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



<img width="1351" alt="Screenshot at Jul 29 20-48-48" src="https://github.com/user-attachments/assets/499daea4-753e-4779-b01d-7c35b69c576c">
<img width="1351" alt="Screenshot at Jul 29 20-47-49" src="https://github.com/user-attachments/assets/b242f149-4fac-4268-9885-cfeaeeb33349">
<img width="1351" alt="3" src="https://github.com/user-attachments/assets/85e00d3a-37f4-4a3f-9d84-36f62c52c6a1">
<img width="1351" alt="1" src="https://github.com/user-attachments/assets/46b00775-030e-48db-a095-3cc14b46ce98">
<img width="1351" alt="Screenshot at Jul 29 20-47-18" src="https://github.com/user-attachments/assets/2e7ce67d-dba9-4b84-ae5c-73e8b53990d4">
<img width="1351" alt="Screenshot at Jul 29 20-48-24" src="https://github.com/user-attachments/assets/c95d72a9-2032-4f4d-bbcb-c73dd918668b">
<img width="1351" alt="Screenshot at Jul 29 20-48-12" src="https://github.com/user-attachments/assets/7bf28be1-c6b7-4ab7-9949-778f66a46897">
<img width="1351" alt="Screenshot at Jul 29 20-48-00" src="https://github.com/user-attachments/assets/c83e398c-182f-4121-906b-c306516a7afa">
<img width="1351" alt="Screenshot at Jul 29 20-48-57" src="https://github.com/user-attachments/assets/5b09981e-5262-42d8-85ce-1c9258094914">
