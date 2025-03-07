using System;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data;

/// <summary>
/// Veritabanı işlemleri için uzantı metotları
/// </summary>
public static class DataExtensions
{
    /// <summary>
    /// Veritabanı migrasyon işlemini gerçekleştirir
    /// </summary>
    /// <param name="app">Web uygulaması</param>
    /// <returns>Migrasyon işleminin tamamlanmasını bekleyen görev</returns>
    public static async Task MigrateDBAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
