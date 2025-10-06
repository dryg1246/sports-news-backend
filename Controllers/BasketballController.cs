using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketballController : Controller
{
    public readonly SportsNewsContext _context;
    public readonly NewsService _newsService;


    public BasketballController(SportsNewsContext context, NewsService newsService)
    {
        _context = context;
        _newsService = newsService;
    }
    
    
    [HttpGet("/loadBasketballNews")]
    public async Task<IActionResult> LoadBasketballNews()
    {
        await _newsService.LoadBasketballNews();
        return Ok("basketball news loaded");
    }

    [HttpGet("/getBasketballNews/{category}")]
    public async Task<IActionResult> GetBasketballNews(string category)
    {
        var basketballNews = await _context.Articles.Where(a => a.Category == category).ToListAsync();
        return Ok(basketballNews);
    }


}