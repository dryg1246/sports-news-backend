using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class ChessController : Controller
{
    public readonly SportsNewsContext _context;
    public readonly NewsService _newsService;

    public ChessController(NewsService newsService, SportsNewsContext context)
    {
        _newsService = newsService;
        _context = context;
    }
    
    [HttpGet("/loadChessNews")]
    public async Task<IActionResult> LoadChessNews()
    {
        await _newsService.LoadChessNews();
        return Ok("News success");
    }

    [Microsoft.AspNetCore.Mvc.HttpGet("/getChessNews/{category}")]
    public async Task<IActionResult> GetChessNews(string category)
    {
        var chessNews = await _context.Articles.Where(c => c.Category == category).ToListAsync();
        if (chessNews == null)
        {
            return BadRequest("Новостей не найденно");
        }
        return Ok(chessNews);
    }
}