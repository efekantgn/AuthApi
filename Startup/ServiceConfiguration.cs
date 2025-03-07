using AuthAPI.Data;
using AuthAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Uygulama servislerinin yapılandırma sınıfı
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    /// Uygulama servislerini yapılandırır
    /// </summary>
    /// <param name="services">Servis koleksiyonu</param>
    /// <param name="configuration">Uygulama yapılandırması</param>
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Auth");
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSqlite<AuthDbContext>(connectionString);
        services.AddScoped<AuthService>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }
}