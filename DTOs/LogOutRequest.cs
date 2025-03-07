using System;

namespace AuthAPI.DTOs;

/// <summary>
/// Çıkış isteği için veri transfer nesnesi
/// </summary>
public class LogOutRequest
{
    /// <summary>
    /// Çıkış yapacak kullanıcının ID'si
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Kullanıcının yenileme token'ı
    /// </summary>
    public required string RefreshToken { get; set; }
}
