using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Interfaces;

namespace SportsNewsAPI.Infractructure;

public class UserRepository : IUserRepository
{
    private readonly SportsNewsContext _context;
    private readonly IMapper _mapper;

    public UserRepository(SportsNewsContext context)
    {
        _context = context;
    }
    public async Task<User> GetByEmail(string email)
    {
        var userEntity = await _context.User.AsNoTracking().FirstOrDefaultAsync(e => e.Email == email) ??
                         throw new Exception();

        return userEntity;
    }

    public string? GetEmailFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                return null;
            }

            var purpose = jwtToken.Claims.FirstOrDefault(c => c.Type == "purpose")?.Value;
            if (purpose != "password-reset")
            {
                return null;
            }

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return email;
        }
        catch
        {
            return null;
        }
    }
}