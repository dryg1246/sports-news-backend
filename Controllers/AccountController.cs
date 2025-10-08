using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using SportsNewsAPI.Dtos;
using SportsNewsAPI.Interfaces;
using SportsNewsAPI.Interfaces.JWT;
using SportsNewsAPI.Request.User;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly UserServices _userServices;
    private readonly SportsNewsContext _context;
    private readonly IEmailServices _emailServices;
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerate _jwtGenerate;

    public AccountController(IEmailServices emailServices, IJwtGenerate jwtGenerate, UserServices userServices,
        SportsNewsContext context, IUserRepository userRepository)
    {
        _userServices = userServices;
        _context = context;
        _userRepository = userRepository;
        _jwtGenerate = jwtGenerate;
        _emailServices = emailServices;
    }

    [HttpPost("/register")]
    public async Task<ActionResult> Register([FromBody] RegisterUserDto dto)
    {
        await _userServices.Register(dto);

        return Ok("User registered successfully");
    }

    [HttpPost("/login")]
    public async Task<ActionResult> Login([FromBody] LoginUserDto dto)
    {
        var token = await _userServices.Login(dto);

        return Ok(token);
    }

    [HttpGet("/allUser")]
    public async Task<ActionResult> GetALlUser()
    {
        var users = await _context.User.ToListAsync();

        return Ok(users);
    }

    [HttpPost("/forgot-password")]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        var user = _userRepository.GetByEmail(dto.EmailTo);
    
        if (user == null)
        {
            return BadRequest("User not found");
        }
    
        var token = _jwtGenerate.GeneratePasswordResetToken(await user);
        
        var resetLink = $"http://localhost:5173/reset-password?token={Uri.EscapeDataString(token)}";
        
        var body = $@"
        <p>Hi {dto.EmailTo}, ScoreXI your favorite website</p>
        <p>Click the link below to reset your password:</p>
        <a href='{resetLink}'>Reset Password</a>
        <p>This link will expire in 15 minutes.</p>";
        
        
        await _emailServices.SendEmail(dto, body);

        return Ok();
    }
}