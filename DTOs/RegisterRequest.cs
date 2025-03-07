using System;

namespace AuthAPI.DTOs;

/// <summary>
/// Yeni kullanıcı kaydı için veri transfer nesnesi
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Yeni kullanıcının e-posta adresi
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Yeni kullanıcının şifresi
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Kullanıcının rolü (varsayılan: "User")
    /// </summary>
    public string Role { get; set; } = "User";
}
