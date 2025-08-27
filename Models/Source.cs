using System.Text.Json.Serialization;

namespace SportsNewsAPI;


public class Source
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonPropertyName("id")]
    public string ApiId { get; set; } = string.Empty;  // id из NewsAPI

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}