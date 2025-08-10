namespace SportsNewsAPI;

public class MatchEvent
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public Match Match { get; set; }
    
    public int Minute { get; set; }
    public string Type { get; set; }
    public string PlayerName { get; set; }
    public string Description { get; set; }
}