using System.Text;
using AuthAPI.Data;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// **1. Configuration ve Database Bağlantısı**
var ConnectionString = configuration.GetConnectionString("Auth");
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddSqlite<AuthDbContext>(ConnectionString);
builder.Services.AddScoped<AuthService>();

// **2. Controllers ve Swagger Servisi**
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
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

    // **JWT Yetkilendirme Ekleme**
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
});

// **3. JWT Authentication Yapılandırması**
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

var app = builder.Build();

// **4. Swagger Middleware**
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API v1");
    });
}

// **5. Middleware Sırası Önemlidir**
app.UseAuthentication(); // **JWT Authentication'ı devreye sok**
app.UseAuthorization();  // **Yetkilendirme Middleware'ini ekle**

app.MapControllers();
await app.MigrateDBAsync();
app.Run();
