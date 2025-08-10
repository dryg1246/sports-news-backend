namespace SportsNewsAPI;

public class Sports
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Format  { get; set; }
    public string? IconUrl  { get; set; }
    public string? Description  { get; set; }
    public ICollection<League> Leagues { get; set; }
}