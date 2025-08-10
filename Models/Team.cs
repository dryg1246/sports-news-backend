namespace SportsNewsAPI;

public class Team
{
    public int Id  { get; set; }
    public int LeagueId { get; set; }
    public League League { get; set; }
    
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string Country { get; set; }
}