using System.Text.Json.Serialization;

namespace SportsNewsAPI.Dtos;

public class SportsDto
{
    [JsonPropertyName("idSport")]
    public int IdDto { get; set; }
    [JsonPropertyName("strSport")]
    public string NameDto { get; set; }

    [JsonPropertyName("strFormat")]
    public string FormatDto { get; set; }

    [JsonPropertyName("strSportThumb")]
    public string IconUrlDto { get; set; }

    [JsonPropertyName("strSportDescription")]
    public string DescriptionDto { get; set; }
    public ICollection<League> Leagues { get; set; }
}