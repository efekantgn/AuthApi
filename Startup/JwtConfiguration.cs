using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

/// <summary>
/// JWT kimlik doğrulama yapılandırma sınıfı
/// </summary>
public static class JwtConfiguration
{
    /// <summary>
    /// JWT kimlik doğrulama servislerini yapılandırır
    /// </summary>
    /// <param name="services">Servis koleksiyonu</param>
    /// <param name="configuration">Uygulama yapılandırması</param>
    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    ValidateIssuerSigningKey = true
                };
            });
    }
}