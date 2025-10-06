using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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
    
    public async Task Register(string userName, string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(Guid.NewGuid(), userName, email, hashedPassword);

        _context.User.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);

        var results = _passwordHasher.Verify(password, user.PasswordHash);

        if (results == false)
        {
            throw new Exception("Fail To Login");
        }
        if (user.Email != email)
        {
            Console.WriteLine("Пользователь не зарегестрирован");
        }


        var token = _jwtGenerate.GenerateToken(user);
        return token;
    }
}