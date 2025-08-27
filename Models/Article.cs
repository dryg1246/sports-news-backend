using System.Text.Json.Serialization;

namespace SportsNewsAPI;

public class Article
{
    public int Id { get; set; }

    public int NewsId { get; set; }       
    public News News { get; set; }       

    public int SourceId { get; set; }    
    public Source Source { get; set; }

    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("urlToImage")]
    public string UrlToImage { get; set; } = string.Empty;

    [JsonPropertyName("publishedAt")]
    public DateTime PublishedAt { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}