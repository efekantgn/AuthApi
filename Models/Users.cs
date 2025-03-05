using System;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models;

public class User
{
    public int Id { get; set; }

    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string PasswordHash { get; set; }

    [Required]
    public string Role { get; set; } = "User"; // Varsayılan olarak 'User' rolü

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
