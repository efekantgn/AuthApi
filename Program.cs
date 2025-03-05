using System.Text;
using AuthAPI.Data;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Servislerin yapılandırılması
builder.Services.ConfigureServices(configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJwtAuthentication(configuration);

var app = builder.Build();

// Middleware yapılandırması
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API v1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
await app.MigrateDBAsync();
app.Run();
