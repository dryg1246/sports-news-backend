using SportsNewsAPI.Dtos;

namespace SportsNewsAPI.Interfaces;

public interface IEmailServices
{
     Task SendEmail(ForgotPasswordDto dto, string body);
}