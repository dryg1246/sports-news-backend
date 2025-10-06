using Microsoft.AspNetCore.Mvc;
using SportsNewsAPI.Request.User;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly UserServices _userServices;

    public UserController(UserServices userServices)
    {
        _userServices = userServices;
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
    
}