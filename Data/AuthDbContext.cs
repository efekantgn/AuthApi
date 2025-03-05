using System;
using AuthAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options)
        : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}
