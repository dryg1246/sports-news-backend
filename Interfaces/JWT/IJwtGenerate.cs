using Microsoft.AspNetCore.Identity;

namespace SportsNewsAPI.Interfaces.JWT;

public interface IJwtGenerate
{
    Task<string> GenerateToken(User user);
    // string GeneratePasswordResetToken(User user);
    // string GeneratePasswordResetToken(IdentityUser? user);
}