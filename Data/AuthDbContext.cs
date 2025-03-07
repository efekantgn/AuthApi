using System;
using AuthAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data;

/// <summary>
/// Kimlik doğrulama veritabanı bağlam sınıfı
/// </summary>
public class AuthDbContext : DbContext
{
    /// <summary>
    /// AuthDbContext yapıcı metodu
    /// </summary>
    /// <param name="options">Veritabanı bağlantı seçenekleri</param>
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Kullanıcılar tablosu
    /// </summary>
    public DbSet<User> Users => Set<User>();
}
