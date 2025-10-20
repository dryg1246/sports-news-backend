using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsNewsAPI.Interfaces.JWT;

namespace SportsNewsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SwaggerAuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtGenerate _jwtGenerate;

    public SwaggerAuthController(UserManager<User> userManager, IJwtGenerate jwtGenerate)
    {
        _userManager = userManager;
        _jwtGenerate = jwtGenerate;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
    {
        var user = await _userManager.FindByEmailAsync(username);
        if (user == null || !(await _userManager.CheckPasswordAsync(user, password)))
            return Unauthorized("Неверный логин или пароль");

        var token = await _jwtGenerate.GenerateToken(user);
        Console.WriteLine("SwaggerAuthController issued token: " + token);
        return Ok(new { access_token = token, token_type = "Bearer" });
    }
}
