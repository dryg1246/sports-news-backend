using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SportsNewsAPI.Enum;
using SportsNewsAPI.Interfaces;
using SportsNewsAPI.Role;

namespace SportsNewsAPI
{
    public class User : IdentityUser<Guid>
    {
        // [JsonProperty("id")]
        // public Guid Id { get; set; }
        //
        // [JsonProperty("userName")]
        // [Required]
        // public string UserName { get; set; }
        //
        // [JsonProperty("email")]
        // [Required]
        // [EmailAddress]
        // public string Email { get; set; }
        //
        // [JsonProperty("passwordHash")]
        // [Required]
        // public string PasswordHash { get; set; }
        //
        // [JsonConstructor]
        // public User(Guid id, string userName, string email, string passwordHash)
        // {
        //     Id = id;
        //     UserName = userName;
        //     Email = email;
        //     PasswordHash = passwordHash;
        // }
        // public string FullName { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // public string Role { get; set; }
        //
        // public User(string username) : base(username)
        // {
        //     Role = "User";
        //     CreatedAt = DateTime.UtcNow;
        // }
        
        public User() : base() { }


        // public static User Create(Guid id, string userName, string email, string passwordHash)
        // {
        //     return new User(id, userName, email, passwordHash);
        // }
    }
}