namespace SportsNewsAPI;

public class Team
{
        public int Id { get; set; }
        // public int ExternalId { get; set; }
        public string FullName { get; set; }
        public string Abbreviation { get; set; }
        public string City { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
}