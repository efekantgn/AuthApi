using System;

namespace AuthAPI.DTOs;

public class RefreshTokenRequest
{
    public required int UserId { get; set; }
    public required string RefreshToken { get; set; }
}
