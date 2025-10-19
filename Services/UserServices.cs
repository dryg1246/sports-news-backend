using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsNewsAPI.Dtos;
using SportsNewsAPI.Interfaces;
using SportsNewsAPI.Interfaces.Auth;
using SportsNewsAPI.Interfaces.JWT;

namespace SportsNewsAPI.Services;

public class UserServices
{
    private readonly IJwtGenerate _jwtGenerate;
    private readonly UserManager<User> _userManager;
    public UserServices(UserManager<User> userManager, IJwtGenerate jwtGenerate)
    {
        _jwtGenerate = jwtGenerate;
        _userManager = userManager;
    }
    
    public async Task<IdentityResult> Register(RegisterUserDto dto)
    {
        // var hashedPassword = _passwordHasher.Generate(dto.Password);
        var user = new User()
        {
            UserName = dto.UserName,
            Email = dto.Email,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }
        
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        return result;
    }

    public async Task<string> Login(LoginUserDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            throw new Exception("Пользователь не найден");
        }

        // var passwordHashed = _userManager.PasswordHasher.HashPassword(user, dto.Password);

        var results = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (results == false)
        {
            throw new Exception("Fail To Login");
        }
        if (user.Email != dto.Email)
        {
            Console.WriteLine("Пользователь не зарегестрирован");
        }


        var token = _jwtGenerate.GenerateToken(user);
        return token;
    }
}