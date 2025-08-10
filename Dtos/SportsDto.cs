namespace SportsNewsAPI.Dtos;

public class SportsDto
{
    public int IdDto { get; set; }
    public string NameDto { get; set; }
    public string? FormatDto  { get; set; }
    public string? IconUrlDto  { get; set; }
    public string? DescriptionDto  { get; set; }
    public ICollection<League> Leagues { get; set; }
}