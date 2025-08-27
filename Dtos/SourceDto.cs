using System.Text.Json.Serialization;

namespace SportsNewsAPI.Dtos;

public class SourceDto
{
    [JsonPropertyName("id")]
    public string ApiId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}