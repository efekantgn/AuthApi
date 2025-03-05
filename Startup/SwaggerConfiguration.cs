using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Auth API",
                Version = "v1",
                Description = "JWT Tabanlı Kimlik Doğrulama ve Yetkilendirme API'si",
                Contact = new OpenApiContact
                {
                    Name = "Senin Adın",
                    Email = "email@example.com",
                    Url = new Uri("https://github.com/kullaniciadi")
                }
            });

            ConfigureSwaggerJwtAuth(c);
        });
    }

    private static void ConfigureSwaggerJwtAuth(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Token'ınızı giriniz. (Örn: Bearer {token})",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}