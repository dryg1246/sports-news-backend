using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
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
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("purpose", "password-reset"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var credentials = new SigningCredentials(securityKey, algorithms);
        var token = new JwtSecurityToken
        (
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: credentials
        );
            
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    // public string GeneratePasswordResetToken(IdentityUser<Guid> user)
    // {
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     var key = Encoding.UTF8.GetBytes(_secret);
    //
    //     var securityKey = new SymmetricSecurityKey(key);
    //     var algorithms = SecurityAlgorithms.HmacSha256;
    //     
    //     var credentials = new SigningCredentials(securityKey, algorithms);
    //
    //     Claim[] claims = new[]
    //     {
    //         new Claim(JwtRegisteredClaimNames.Email, user.Email),
    //         new Claim("purpose", "password-reset"),
    //         new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
    //     };
    //
    //     var tokenDescriptor = new SecurityTokenDescriptor
    //     {
    //         Subject = new ClaimsIdentity(claims),
    //         Expires = DateTime.UtcNow.AddMinutes(15),
    //         SigningCredentials = credentials
    //     };
    //
    //     var token = tokenHandler.CreateToken(tokenDescriptor);
    //     return tokenHandler.WriteToken(token);
    // }
}