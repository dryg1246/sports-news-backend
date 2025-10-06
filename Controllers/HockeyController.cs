using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HockeyController : Controller
{
    public readonly SportsNewsContext _context;
    public readonly NewsService _newsService;


    public HockeyController(SportsNewsContext context, NewsService newsService)
    {
        _context = context;
        _newsService = newsService;
    }
    
    
    [HttpGet("/loadHockeyNews")]
    public async Task<IActionResult> LoadHockeyNews()
    {
        await _newsService.LoadHockeyNews();
        return Ok("hockey sports loaded");
    }

    [HttpGet("/getHockeyNews/{category}")]
    public async Task<IActionResult> GetHockeyNews(string category)
    {
        var hockeyNews = await _context.Articles.Where(a => a.Category == category).ToListAsync();
        if (hockeyNews == null)
        {
            return NotFound();
        }
        
        return Ok(hockeyNews);
    }


}