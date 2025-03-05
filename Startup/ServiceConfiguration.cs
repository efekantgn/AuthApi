using AuthAPI.Data;
using AuthAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceConfiguration
{
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