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
            var url = "https://www.thesportsdb.com/api/v1/json/3/all_sports.php";
            var responce = await _httpClient.GetAsync(url);
        
            if (!responce.IsSuccessStatusCode)
                throw new Exception("API request failed");
        
            var result = JsonSerializer.Deserialize<SportsApiResponse>(responce.Content.ReadAsStringAsync().Result);

            if (result?.Sports != null)
            {
                foreach (var sportDto in result.Sports)
                {
                    var existing = await _context.Sports.FirstOrDefaultAsync(sport => sport.Name == sportDto.NameDto);

                    if (existing == null)
                    {
                        var testSport = new Sports
                        {
                            Name = "TestSport",
                            Format = "TestFormat",
                            IconUrl = "http://example.com/icon.png",
                            Description = "Test description"
                        };
                    
                        _context.Sports.Add(testSport);
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
            throw new Exception($"Failed to load sports: {ex.Message}", ex);
        }
        
        
        
    }

}