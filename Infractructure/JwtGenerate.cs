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
    private readonly UserManager<User> _userManager;

    public JwtGenerate(IConfiguration config, UserManager<User> userManager)
    {
        _secret = config["JWT_SECRET"] ?? throw new Exception("JWT_SECRET не найден в переменных окружения!");
        _userManager = userManager;
    }
    public async Task<string> GenerateToken(User user)
    {
        var data = Encoding.UTF8.GetBytes(_secret);

        var securityKey = new SymmetricSecurityKey(data);

        var algorithms = SecurityAlgorithms.HmacSha256;

        var roles =  await _userManager.GetRolesAsync(user);
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("purpose", "password-reset"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var credentials = new SigningCredentials(securityKey, algorithms);
        var token = new JwtSecurityToken
        (
            // issuer: _configuration("Jwt:Issuer"),
            // audience:  _configuration("Jwt:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
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