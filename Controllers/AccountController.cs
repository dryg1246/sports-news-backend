using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using SportsNewsAPI.Dtos;
using SportsNewsAPI.Interfaces;
using SportsNewsAPI.Interfaces.Auth;
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
    private readonly IPasswordHasher _passwordHasher;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IPasswordHasher passwordHasher, IEmailServices emailServices, IJwtGenerate jwtGenerate, UserServices userServices,
        SportsNewsContext context, IUserRepository userRepository)
    {
        _userServices = userServices;
        _context = context;
        _userRepository = userRepository;
        _jwtGenerate = jwtGenerate;
        _emailServices = emailServices;
        _passwordHasher = passwordHasher;
        _userManager = userManager;
        _signInManager = signInManager;
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
        var user = await _userManager.FindByEmailAsync(dto.Email);
        await _signInManager.SignInAsync(user, isPersistent: false);

        return Ok(new 
        { 
            Message = "Пользователь успешно зарегистрирован и авторизован",
            Token = token,
            UserId = user.Id,
            CreatedAt = user.CreatedAt,
            Email = user.Email
        });
    }
    
    [HttpGet("/allUser")]
    public async Task<ActionResult> GetALlUser()
    {
        var users = await _context.User.ToListAsync();

        return Ok(users);
    }

    [HttpGet("/user/{id}")]
    public async Task<ActionResult> GetUserById(Guid id)
    {
        var user = await _context.User.FirstOrDefaultAsync(a => a.Id == id);

        return Ok(user);
    }

    [HttpPatch("/editUser/{id}")]
    public async Task<ActionResult> EditUser(Guid id, [FromBody] UserUpdateDto dto)
    {
        var user = await _context.User.FirstOrDefaultAsync(a => a.Id == id);

        if (user == null)
        {
            return BadRequest("Problem with User: User is null");
        }

        if (!string.IsNullOrEmpty(dto.Email))
        {
            user.Email = dto.Email;
            user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
        }

        if (!string.IsNullOrEmpty(dto.UserName))
        {
            user.UserName = dto.UserName;
            user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
        }

        await _context.SaveChangesAsync();
        return Ok(new { user.Email, user.UserName });
    }

    // [Authorize(Roles = "Admin")]
    [HttpDelete("/deleteUser/{id}")]
    public async Task<string> DeleteUser(Guid id)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return ("User is possible null");
        }

        _context.RemoveRange(user);
        await _context.SaveChangesAsync();
        return ("Users successful delete");
    }

    [HttpPost("/forgot-password")]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.EmailTo);
    
        if (user == null)
        {
            return BadRequest("User not found");
        }
    
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var resetLink = $"http://localhost:5173/reset-password?/email=${dto.EmailTo}token={Uri.EscapeDataString(token)}";
        
        var body = $@"
        <p>Hi {dto.EmailTo}, ScoreXI your favorite website</p>
        <p>Click the link below to reset your password:</p>
        <a href='{resetLink}'>Reset Password</a>
        <p>This link will expire in 15 minutes.</p>";
        
        
        await _emailServices.SendEmail(dto, body);

        return Ok();
    }

    [HttpPost("/reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto dto)
    {
        if (dto.NewPassword != dto.RepeatNewPassword)
        {
           return BadRequest("Password dont match");
        }
        // var email = _userRepository.GetEmailFromToken(dto.Token);
        //
        // if (email == null)
        // {
        //     return BadRequest("Email dont founded");
        // }

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return BadRequest("Не правильный жмейл");
        }
        
        var identityToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, identityToken, dto.NewPassword);
        if (!result.Succeeded)
            return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.UpdateAsync(user);
        return Ok("Пароль изменен");
    }
    
}