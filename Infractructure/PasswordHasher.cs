
using SportsNewsAPI.Interfaces.Auth;

namespace SportsNewsAPI.Infractructure;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool Verify(string hashedPassword, string password) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
}