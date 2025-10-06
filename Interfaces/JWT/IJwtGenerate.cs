namespace SportsNewsAPI.Interfaces.JWT;

public interface IJwtGenerate
{
    string GenerateToken(User user);
}