using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Request.User;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly UserServices _userServices;
    private readonly SportsNewsContext _context;

    public AccountController(UserServices userServices, SportsNewsContext context)
    {
        _userServices = userServices;
        _context = context;
    }
    
    [HttpPost("/register")]
    public async Task<ActionResult> Register([FromBody]RegisterUserRequest request)
    {
        await _userServices.Register(request.UserName, request.Email, request.Password);

        return Ok("User registered successfully");
    }

    [HttpPost("/login")]
    public async Task<ActionResult> Login([FromBody]LoginUserRequest request)
    {
        var token = await _userServices.Login(request.Email, request.Password);

        return Ok(token);
    }

    [HttpGet("/allUser")]
    public async Task<ActionResult> GetALlUser()
    {
        var users = await _context.User.ToListAsync();

        return Ok(users);
    }
    
}