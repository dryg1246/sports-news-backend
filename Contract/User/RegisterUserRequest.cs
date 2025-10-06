namespace SportsNewsAPI.Request.User;

public record RegisterUserRequest(string UserName, string Email, string Password);