using System;

namespace AuthAPI.DTOs;

/// <summary>
/// Token yanıtı için veri transfer nesnesi
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// JWT erişim token'ı
    /// </summary>
    public required string AccesToken { get; set; }

    /// <summary>
    /// Yenileme token'ı
    /// </summary>
    public required string RefreshToken { get; set; }
}
