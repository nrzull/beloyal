using System.Globalization;
using Beloyal.API.Auth;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API;

public class ApplicationDbContext(DbContextOptions opts) : DbContext(opts)
{
  public DbSet<AuthEmailOTP> AuthEmailOTPs { get; set; }
  public DbSet<User> Users { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
    base.OnConfiguring(builder);
    builder.UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture);
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
  }

  public static string GetConnectionString()
  {
    var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
    var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
    var database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");
    var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    var poolSize = Environment.GetEnvironmentVariable("POSTGRES_POOL_SIZE");

    return $"Host={host}; Port={port}; Database={database}; Username={user}; Password={password}; Maximum Pool Size={poolSize}";
  }
}
