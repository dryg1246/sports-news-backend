using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SportsNewsAPI.Interfaces.JWT;

namespace SportsNewsAPI.Infractructure;

public class JwtGenerate : IJwtGenerate
{

    private readonly string? _secret;

    public JwtGenerate(IConfiguration config)
    {
        _secret = config["JWT_SECRET"] ?? throw new Exception("JWT_SECRET не найден в переменных окружения!");
    }
    public string GenerateToken(User user)
    {
        var data = Encoding.UTF8.GetBytes(_secret);

        var securityKey = new SymmetricSecurityKey(data);

        var algorithms = SecurityAlgorithms.HmacSha256;

        Claim[] claims = [new("userId", user.Id.ToString())];
        
        
        var credentials = new SigningCredentials(securityKey, algorithms);
        var token = new JwtSecurityToken
        (
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: credentials
        );
            
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}