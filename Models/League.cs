namespace SportsNewsAPI;

public class League
{
    public int Id { get; set; }
    public int SportId { get; set; }
    public Sports Sport { get; set; }
    
    public string Name { get; set; }
    public string Country { get; set; }
    public string IconUrl { get; set; }
    public ICollection<Team> Teams { get; set; }
}