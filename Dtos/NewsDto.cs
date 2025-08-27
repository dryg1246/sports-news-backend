using System.Text.Json.Serialization;

namespace SportsNewsAPI.Dtos;

public class NewsDto
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("totalResults")]
    public int TotalResults { get; set; }

    [JsonPropertyName("articles")]
    public List<ArticleDto> Articles { get; set; } = new List<ArticleDto>();
}