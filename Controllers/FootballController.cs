using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class FootballController : Controller
{
    public readonly SportsNewsContext _context;
    public readonly NewsService _newsService;


    public FootballController(SportsNewsContext context, NewsService newsService)
    {
        _context = context;
        _newsService = newsService;
    }
    
    
    [Authorize(Roles = "Admin")]
    [Microsoft.AspNetCore.Mvc.HttpGet("/loadFootballNews")]
    public async Task<IActionResult> LoadFootballNews()
    {
        await _newsService.LoadFootballNews();
        return Ok("sports loaded");
    }
    
    [Microsoft.AspNetCore.Mvc.HttpGet("/getNews/{category}")]
    public async Task<IActionResult> GetFootballNews(string category)
    {
        var footballNews = await _context.Articles.Where(a => a.Category == category).ToListAsync();
        if (footballNews == null)
        {
            return NotFound();
        }
        return Ok(footballNews);
    }

}