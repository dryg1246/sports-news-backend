using System.IdentityModel.Tokens.Jwt;
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
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly SportsNewsContext _context;
    private readonly IJwtGenerate _jwtGenerate;
    public UserServices(IJwtGenerate jwtGenerate, IPasswordHasher passwordHasher, IUserRepository userRepository, SportsNewsContext context)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _context = context;
        _jwtGenerate = jwtGenerate;
    }
    
    public async Task Register(RegisterUserDto dto)
    {
        var hashedPassword = _passwordHasher.Generate(dto.Password);

        var user = User.Create(Guid.NewGuid(), dto.UserName, dto.Email, hashedPassword);

        _context.User.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<string> Login(LoginUserDto dto)
    {
        var user = await _userRepository.GetByEmail(dto.Email);

        var results = _passwordHasher.Verify(dto.Password, user.PasswordHash);

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