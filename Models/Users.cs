using System;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models;

/// <summary>
/// Kullanıcı bilgilerini temsil eden sınıf
/// </summary>
public class User
{
    /// <summary>
    /// Kullanıcının benzersiz kimlik numarası
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Kullanıcının e-posta adresi (giriş için kullanılır)
    /// </summary>
    [Required, EmailAddress]
    public required string Email { get; set; }

    /// <summary>
    /// Kullanıcının şifresinin hash'lenmiş hali
    /// </summary>
    [Required]
    public required string PasswordHash { get; set; }

    /// <summary>
    /// Kullanıcının rolü (yetkilendirme için kullanılır)
    /// </summary>
    [Required]
    public string Role { get; set; } = "User"; // Varsayılan olarak 'User' rolü

    /// <summary>
    /// Kullanıcının yenileme token'ı (oturum yenileme için kullanılır)
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Yenileme token'ının son geçerlilik tarihi
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
