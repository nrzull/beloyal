using System.Globalization;
using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API;

public class ApplicationDbContext(DbContextOptions opts) : DbContext(opts)
{
  public DbSet<AuthEmailOTP> AuthEmailOTPs { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<Company> Companies { get; set; }
  public DbSet<CompanyAccess> CompanyAccesses { get; set; }
  public DbSet<CompanyRole> CompanyRoles { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
    base.OnConfiguring(builder);
    builder.UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture);
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<User>(user =>
    {
      user.Property(v => v.Email).HasColumnType("citext").HasMaxLength(64);
      user.HasIndex(v => v.Email).IsUnique();
    });

    builder.Entity<Company>(company =>
    {
      company.Property(v => v.Tin).HasMaxLength(12);
      company.HasIndex(v => v.Tin).IsUnique();
    });

    builder.Entity<CompanyAccess>(companyAccess =>
    {
      companyAccess
        .HasOne(v => v.Company)
        .WithMany(v => v.Accesses)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);

      companyAccess
        .HasOne(v => v.Role)
        .WithMany()
        .HasForeignKey(v => v.RoleId)
        .HasPrincipalKey(v => v.Id);

      companyAccess
        .HasOne(v => v.User)
        .WithMany(v => v.CompanyAccesses)
        .HasForeignKey(v => v.UserId)
        .HasPrincipalKey(v => v.Id);
    });

    builder.Entity<CompanyRole>(companyRole =>
    {
      companyRole
        .HasOne(v => v.Company)
        .WithMany(v => v.Roles)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);
    });
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
