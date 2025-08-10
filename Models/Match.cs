namespace SportsNewsAPI;

public class Match
{
    public int Id { get; set; }
    public int LeagueId { get; set; }
    public League League { get; set; }
    
    public int HomeTeamId { get; set; }
    public Team HomeTeam { get; set; }
    
    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; }
    
    public DateTime StartTime { get; set; }
    public string Status { get; set; }
    public string? ScoreHome { get; set; }
    public string? ScoreAway { get; set; }
}