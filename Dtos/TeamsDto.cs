using System.Text.Json.Serialization;

namespace SportsNewsAPI.Dtos;

public class TeamDto
{
        [JsonPropertyName("id")]
        public int Id { get; set; }
    
        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = null!;
    
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; } = null!;
    
        [JsonPropertyName("city")]
        public string City { get; set; } = null!;
    
        [JsonPropertyName("conference")]
        public string Conference { get; set; } = null!;
    
        [JsonPropertyName("division")]
        public string Division { get; set; } = null!;
}