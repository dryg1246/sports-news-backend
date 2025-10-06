using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Services;

namespace SportsNewsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SportsController : ControllerBase
{
    
    private readonly SportsNewsContext _context;
    private readonly SportService _sportService;

    public SportsController(SportsNewsContext context, SportService sportService)
    {
        _context = context;
        _sportService = sportService;
    }
    
    [HttpGet("load")]
    public async Task<ActionResult> LoadSports()
    {
        await _sportService.LoadSportsAsync();
        return Ok("sports loaded"); 
    }
    
    
    [HttpGet("loadTeam")]
    public async Task<ActionResult> LoadTeams()
    {
         await _sportService.LoadTeamAsync();
        return Ok("team loaded"); 
    }

    [HttpGet("/sports")]
    public async Task<ActionResult<IEnumerable<Sports>>> GetSports()
    {
        var sports = await _context.Sports.ToListAsync();
        return Ok(sports);
    }

    [HttpGet("/teams")]
    public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
    {
        var teams = await _context.Teams.ToListAsync();
        if (teams == null)
        {
            return NotFound();
        }
        return Ok(teams);
    }

    
    [HttpGet("testadd")]
    public async Task<IActionResult> TestAdd()
    {
        var sport = new Sports
        {
            Name = "TestSport",
            Format = "TestFormat",
            IconUrl = "https://example.com/icon.png",
            Description = "Test description"
        };

        _context.Sports.Add(sport);
        await _context.SaveChangesAsync();

        return Ok("Test sport added");
    }

    [HttpGet("testTeamAdd")]
    public async Task<IActionResult> TestTeamAdd()
    {
        var team = new Team
        {
            FullName = "Danya",
            Abbreviation = "nba",
            City = "New York",
            Conference = "New York",
            Division = "NBA",
        };
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        
        return Ok("Test team added");
    }
    
  


}