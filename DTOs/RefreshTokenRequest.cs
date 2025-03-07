using System;

namespace AuthAPI.DTOs;

/// <summary>
/// Token yenileme isteği için veri transfer nesnesi
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Token'ı yenilenecek kullanıcının ID'si
    /// </summary>
    public required int UserId { get; set; }

    /// <summary>
    /// Mevcut yenileme token'ı
    /// </summary>
    public required string RefreshToken { get; set; }
}
