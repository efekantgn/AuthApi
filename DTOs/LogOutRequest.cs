using System;

namespace AuthAPI.DTOs;

public class LogOutRequest
{
    public int UserId { get; set; }
    public required string RefreshToken { get; set; }
}
