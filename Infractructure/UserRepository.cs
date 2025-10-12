using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Interfaces;
using JwtHeaderParameterNames = Microsoft.IdentityModel.JsonWebTokens.JwtHeaderParameterNames;

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
        var handleToken = new JwtSecurityTokenHandler();

        try
        {
            var jwtToken = handleToken.ReadToken(token) as JwtSecurityToken;

            if (jwtToken?.ValidTo < DateTime.UtcNow.Date)
            {
                return null;
            }

            var email = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email)?.Value;
            Console.WriteLine(email);
            
            
            var resetPasswordClaims = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "purpose")?.Value;
            if (resetPasswordClaims != "password-reset")
            {
                return null;
            }
            return email ;
        }
        catch 
        {
            return null;
        }
        
        
    }
}