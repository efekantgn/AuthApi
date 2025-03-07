using System;

namespace AuthAPI.DTOs;

/// <summary>
/// Kullanıcı giriş isteği için veri transfer nesnesi
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Kullanıcının e-posta adresi
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Kullanıcının şifresi
    /// </summary>
    public required string Password { get; set; }
}
