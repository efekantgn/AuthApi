using AuthAPI.Data;
using AuthAPI.Data.Migrations;
using AuthAPI.DTOs;
using AuthAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Services;

/// <summary>
/// Kimlik doğrulama ve yetkilendirme işlemlerini yöneten servis sınıfı
/// </summary>
public class AuthService
{
    private readonly AuthDbContext _context;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// AuthService sınıfının yapıcı metodu
    /// </summary>
    /// <param name="dbContext">Veritabanı bağlam nesnesi</param>
    /// <param name="configuration">Uygulama yapılandırma nesnesi</param>
    public AuthService(AuthDbContext dbContext, IConfiguration configuration)
    {
        _context = dbContext;
        _configuration = configuration;
    }

    /// <summary>
    /// Yeni bir kullanıcı kaydı oluşturur
    /// </summary>
    /// <param name="request">Kayıt isteği bilgileri</param>
    /// <returns>Kayıt işleminin başarılı olup olmadığını belirten boolean değer</returns>
    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return false;

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        User user = new()
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = request.Role
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Kullanıcı girişi yapar ve token oluşturur
    /// </summary>
    /// <param name="request">Giriş isteği bilgileri</param>
    /// <returns>JWT token ve yenileme token'ını içeren TokenResponse nesnesi</returns>
    /// <exception cref="ArgumentException">Geçersiz giriş bilgileri durumunda fırlatılır</exception>
    /// <exception cref="UnauthorizedAccessException">Kimlik doğrulama başarısız olduğunda fırlatılır</exception>
    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("E-posta ve şifre gereklidir.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Geçersiz e-posta veya şifre.");
        }

        TokenResponse response = await CreateTokenResponse(user);

        return response;
    }

    /// <summary>
    /// Kullanıcı için token yanıtı oluşturur
    /// </summary>
    /// <param name="user">Token oluşturulacak kullanıcı</param>
    /// <returns>JWT token ve yenileme token'ını içeren TokenResponse nesnesi</returns>
    private async Task<TokenResponse> CreateTokenResponse(User user)
    {
        return new()
        {
            AccesToken = GenerateJwtToken(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        };
    }

    /// <summary>
    /// Sistemdeki tüm kullanıcıların e-posta adreslerini listeler
    /// </summary>
    /// <returns>Kullanıcı e-posta adreslerini içeren liste</returns>
    public async Task<List<string>> GetUsers()
    {
        return await _context.Users
                .Select(user => user.Email)
                .AsNoTracking()
                .ToListAsync();
    }

    /// <summary>
    /// Yenileme token'ı kullanarak yeni bir JWT token oluşturur
    /// </summary>
    /// <param name="request">Yenileme token isteği bilgileri</param>
    /// <returns>Yeni JWT token ve yenileme token'ını içeren TokenResponse nesnesi veya null</returns>
    public async Task<TokenResponse?> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

        if (user is null)
            return null;

        return await CreateTokenResponse(user);
    }

    /// <summary>
    /// Kullanıcı oturumunu sonlandırır
    /// </summary>
    /// <param name="userId">Kullanıcı kimlik numarası</param>
    /// <param name="refreshToken">Kullanıcının yenileme token'ı</param>
    /// <returns>Oturumu sonlandırılan kullanıcı nesnesi veya null</returns>
    public async Task<User?> LogOutAsync(int userId, string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);

        if (user is null
        || user.RefreshToken != refreshToken
        || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return null;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _context.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Yenileme token'ının geçerliliğini doğrular
    /// </summary>
    /// <param name="UserId">Kullanıcı kimlik numarası</param>
    /// <param name="refreshToken">Doğrulanacak yenileme token'ı</param>
    /// <returns>Doğrulama başarılı ise kullanıcı nesnesi, değilse null</returns>
    private async Task<User?> ValidateRefreshTokenAsync(int UserId, string refreshToken)
    {
        var user = await _context.Users.FindAsync(UserId);
        if (user is null
        || user.RefreshToken != refreshToken
        || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }
        return user;
    }

    /// <summary>
    /// Kullanıcı için JWT token oluşturur
    /// </summary>
    /// <param name="user">Token oluşturulacak kullanıcı</param>
    /// <returns>Oluşturulan JWT token string'i</returns>
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Yeni bir yenileme token'ı oluşturur
    /// </summary>
    /// <returns>Oluşturulan yenileme token'ı</returns>
    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)); // 64 byte'lık random token
    }

    /// <summary>
    /// Kullanıcı için yeni bir yenileme token'ı oluşturur ve kaydeder
    /// </summary>
    /// <param name="user">Token oluşturulacak kullanıcı</param>
    /// <returns>Oluşturulan yenileme token'ı</returns>
    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();
        return refreshToken;
    }
}
