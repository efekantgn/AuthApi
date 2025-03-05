using System;

namespace AuthAPI.DTOs;

public class TokenResponse
{
    public required string AccesToken { get; set; }
    public required string RefreshToken { get; set; }
}
