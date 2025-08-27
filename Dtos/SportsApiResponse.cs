using Newtonsoft.Json;

namespace SportsNewsAPI.Dtos;

public class SportsApiResponse
{
    [JsonProperty("sports")]
    public List<SportsDto> Sports { get; set; }
    [JsonProperty("data")]
    public List<TeamDto> Teams { get; set; }
}