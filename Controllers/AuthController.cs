using AuthAPI.DTOs;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool isRegistered = await _authService.RegisterAsync(request);

            if (!isRegistered)
                return BadRequest(new { message = "E-posta zaten kullanımda." });

            return Ok(new { message = "Kayıt başarılı!" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request);
                return Ok(token);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Geçersiz e-posta veya şifre.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            if (result is null || result!.AccesToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid Refresh Token");

            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<ActionResult> LogOutAsync(LogOutRequest request)
        {
            var user = await _authService.LogOutAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return Unauthorized("Log Out Failed");

            return Ok("Succesfully Loged out");
        }
        [Authorize]
        [HttpGet("isAuthanticated")]
        public IActionResult AuthanticationOnlyEndpoint()
        {
            return Ok("You are authanticated");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are Admin");
        }
    }
}
