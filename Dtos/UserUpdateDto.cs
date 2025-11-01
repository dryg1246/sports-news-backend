using System.ComponentModel.DataAnnotations;

namespace SportsNewsAPI.Dtos;

public class UserUpdateDto
{
    public string? UserName { get; set; }
    
    [EmailAddress(ErrorMessage = "Неверный формат email.")]
    public string? Email { get; set; }
}