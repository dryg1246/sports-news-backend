using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BadmintonController : Controller
{
    public readonly SportsNewsContext _context;
    public readonly NewsService _newsService;


    public BadmintonController(SportsNewsContext context, NewsService newsService)
    {
        _context = context;
        _newsService = newsService;
    }
    
    
    [HttpGet("/loadBadmintonNews")]
    public async Task<IActionResult> LoadBadmintonNews()
    {
        await _newsService.LoadBadmintonNews();
        return Ok("badminton news load");
    }

    [HttpGet("/getBadmintonNews/{category}")]
    public async Task<IActionResult> GetBadmintonNews(string category)
    {
        var badmintonNews = await _context.Articles.Where(a => a.Category == category).ToListAsync();
        return Ok(badmintonNews);
    }

}