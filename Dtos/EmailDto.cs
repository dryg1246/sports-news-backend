using System.ComponentModel.DataAnnotations;

namespace SportsNewsAPI.Dtos;

public class EmailDto
{
    [Required, EmailAddress]
    public string EmailTo { get; set; }
    public string Subject { get; set; }
}