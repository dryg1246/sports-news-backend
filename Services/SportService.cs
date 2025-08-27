using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SportsNewsAPI.Dtos;

namespace SportsNewsAPI.Services;

public class SportService
{
    private readonly HttpClient _httpClient;
    private readonly SportsNewsContext _context;
    private readonly ILogger<SportService> _logger;

    public SportService(HttpClient httpClient, SportsNewsContext context, ILogger<SportService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
    }

    public async Task LoadSportsAsync()
    {
        try
        {
            var url = "https://www.thespportsdb.com/api/v1/json/123/all_sports.ph";
            var response = await _httpClient.GetAsync(url);
        
            if (!response.IsSuccessStatusCode)
                throw new Exception($"API request failed with status {response.StatusCode}");
        
            var result = await response.Content.ReadFromJsonAsync<SportsApiResponse>();
            
            if (result?.Sports == null || result.Sports.Count == 0)
            {
                _logger.LogWarning("No sports data received from API.");
                return;
            }

            if (result?.Sports != null)
            {
                foreach (var sportDto in result.Sports)
                {
                    var existing = await _context.Sports.FirstOrDefaultAsync(sport => sport.Name == sportDto.NameDto);

                    if (existing == null)
                    {
                        var testSport = new Sports
                        {
                            Name = sportDto.NameDto,
                            Format = sportDto.FormatDto,
                            IconUrl = sportDto.IconUrlDto,
                            Description = sportDto.DescriptionDto,
                        };
                        _context.Sports.Add(testSport);
                        _logger.LogInformation("Sports successfully loaded and saved.");
                    }
                    else
                    {
                        existing.Format = sportDto.FormatDto;
                        existing.IconUrl = sportDto.IconUrlDto;
                        existing.Description = sportDto.DescriptionDto;
                    }
                }
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load sports.");
            throw;
        }
        
        
        
    }
      public async Task LoadTeamAsync()
    {
        try
        {
            var url = "https://api.balldontlie.io/v1/teams";
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "dca0e57c-8d49-4004-88d8-a783467aa975");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp/1.0");
            var response = await _httpClient.GetAsync(url);
        
            if (!response.IsSuccessStatusCode)
                throw new Exception($"API request failed with status {response.StatusCode}");

            var result = await response.Content.ReadFromJsonAsync<SportsApiResponse>();
            
            _logger.LogInformation("Received {count} teams from API", result?.Teams?.Count ?? 0);
            
            if (result?.Teams == null || result.Teams.Count == 0)
            {
                _logger.LogWarning("No sports data received from API.");
                return;
            }

            if (result?.Teams != null)
            {
                foreach (var teamsDto in result.Teams)
                {
                    var existing = await _context.Teams.FirstOrDefaultAsync(sport => sport.FullName == teamsDto.FullName);

                    if (existing == null)
                    {
                        var testTeam = new Team
                        {
                            Id = 1,
                            FullName = "Danya SmetaNIN",
                            Abbreviation = "nba",
                            City = "NewYork",
                            Conference = "nba",
                            Division = "1",
                        };
                        _context.Teams.Add(testTeam);
                        _logger.LogInformation("Sports successfully loaded and saved.");
                    }
                    else
                    {
                        existing.FullName = teamsDto.FullName;
                        existing.Abbreviation = teamsDto.Abbreviation;
                        existing.City = teamsDto.City;
                        existing.Conference = teamsDto.Conference;
                        existing.Division = teamsDto.Division;
                    }
                }
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load sports.");
            throw;
        }
        
        
        
    }

}