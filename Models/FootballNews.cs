namespace SportsNewsAPI;

public class FootballNews
{
    public int Id { get; set; }
    public Article Article { get; set; } = new Article(); 
}