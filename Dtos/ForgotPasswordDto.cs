using System.ComponentModel.DataAnnotations;

namespace SportsNewsAPI.Dtos;

public class ForgotPasswordDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    public string? Subject { get; set; }
}