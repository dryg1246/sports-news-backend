using System.ComponentModel.DataAnnotations;

namespace SportsNewsAPI.Dtos;

public class ForgotPasswordDto
{
    [Required, EmailAddress]
    public string EmailTo { get; set; }
    [Required]
    public string Subject { get; set; }
}