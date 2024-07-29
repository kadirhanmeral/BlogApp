namespace BlogApp.Services.Abstract
{
    public interface IPasswordHasher
    {
        string Hash(string password);

        bool Verify(string passwordHash, string password);
    }
}