using Application.Interfaces;

namespace Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return $"hashed_{new string(password.Reverse().ToArray())}";
    }
}