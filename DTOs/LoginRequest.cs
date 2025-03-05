using System;

namespace AuthAPI.DTOs;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
