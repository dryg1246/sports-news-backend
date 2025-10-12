
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SportsNewsAPI.Dtos;
using SportsNewsAPI.Interfaces;

namespace SportsNewsAPI.Services;

public class EmailServices : IEmailServices
{
    public async Task SendEmail(ForgotPasswordDto dto, string body)
    {
        var emailFrom = Environment.GetEnvironmentVariable("EMAIL_FROM");
        var passwordEmail = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(emailFrom));
        email.To.Add(MailboxAddress.Parse(dto.EmailTo));
        email.Subject = dto.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };
        
        
    
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(emailFrom, passwordEmail);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}