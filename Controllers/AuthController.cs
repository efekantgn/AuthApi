using AuthAPI.DTOs;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// Kimlik doğrulama ve yetkilendirme işlemlerini yöneten API controller
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        /// <summary>
        /// AuthController sınıfının yapıcı metodu
        /// </summary>
        /// <param name="authService">Kimlik doğrulama servisi</param>
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Yeni bir kullanıcı kaydı oluşturur
        /// </summary>
        /// <param name="request">Kayıt isteği bilgileri</param>
        /// <returns>Kayıt işleminin sonucunu içeren IActionResult</returns>
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

        /// <summary>
        /// Kullanıcı girişi yapar ve token döndürür
        /// </summary>
        /// <param name="request">Giriş isteği bilgileri</param>
        /// <returns>JWT token ve yenileme token'ını içeren IActionResult</returns>
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

        /// <summary>
        /// Yenileme token'ı kullanarak yeni bir JWT token oluşturur
        /// </summary>
        /// <param name="request">Yenileme token isteği bilgileri</param>
        /// <returns>Yeni JWT token ve yenileme token'ını içeren ActionResult</returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            if (result is null || result!.AccesToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid Refresh Token");

            return Ok(result);
        }

        /// <summary>
        /// Kullanıcı oturumunu sonlandırır
        /// </summary>
        /// <param name="request">Çıkış isteği bilgileri</param>
        /// <returns>Çıkış işleminin sonucunu içeren ActionResult</returns>
        [HttpPost("logout")]
        public async Task<ActionResult> LogOutAsync(LogOutRequest request)
        {
            var user = await _authService.LogOutAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return Unauthorized("Log Out Failed");

            return Ok("Succesfully Loged out");
        }

        /// <summary>
        /// Kimlik doğrulaması gerektiren örnek bir endpoint
        /// </summary>
        /// <returns>Kimlik doğrulaması başarılı mesajını içeren IActionResult</returns>
        [Authorize]
        [HttpGet("isAuthanticated")]
        public IActionResult AuthanticationOnlyEndpoint()
        {
            return Ok("You are authanticated");
        }

        /// <summary>
        /// Sadece Admin rolüne sahip kullanıcıların erişebileceği örnek bir endpoint
        /// </summary>
        /// <returns>Admin rolüne sahip olduğunu belirten mesajı içeren IActionResult</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are Admin");
        }
    }
}
