namespace SportsNewsAPI.Interfaces;

public interface IUserRepository
{
    Task<User> GetByEmail(string email);
    string? GetEmailFromToken(string token);
}