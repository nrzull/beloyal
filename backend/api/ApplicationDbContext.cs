using System.Globalization;
using Beloyal.API.Admin;
using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Seeds;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API;

public class ApplicationDbContext(DbContextOptions opts) : DbContext(opts)
{
  public DbSet<AuthEmailOTP> AuthEmailOTPs { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<AdminAccess> AdminAccesses { get; set; }
  public DbSet<AdminRole> AdminRoles { get; set; }
  public DbSet<WhiteCompany> WhiteCompanies { get; set; }
  public DbSet<Company> Companies { get; set; }
  public DbSet<CompanyAccess> CompanyAccesses { get; set; }
  public DbSet<CompanyRole> CompanyRoles { get; set; }
  public DbSet<CompanyAccessInvitation> CompanyAccessInvitations { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
    base.OnConfiguring(builder);
    builder.UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture);

    if (Environment.GetEnvironmentVariable("API_USE_SEEDING") == "1")
    {
      builder.UseSeeding((dbContext, _) => SeedAdminUser.Run(dbContext));
    }
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<User>(user =>
    {
      user.Property(v => v.Email).HasColumnType("citext").HasMaxLength(64);
      user.HasIndex(v => v.Email).IsUnique();
    });

    builder.Entity<AdminAccess>(adminAccess =>
    {
      adminAccess
        .HasOne(v => v.Role)
        .WithMany()
        .HasForeignKey(v => v.RoleId)
        .HasPrincipalKey(v => v.Id);

      adminAccess
        .HasOne(v => v.User)
        .WithOne(v => v.AdminAccess)
        .HasForeignKey<AdminAccess>(v => v.UserId)
        .HasPrincipalKey<User>(v => v.Id);
    });

    builder.Entity<WhiteCompany>(whiteCompany =>
    {
      whiteCompany.Property(v => v.Tin).HasMaxLength(12);
      whiteCompany.HasIndex(v => v.Tin).IsUnique();
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

      companyAccess.HasIndex(v => new { v.UserId, v.CompanyId }).IsUnique();
    });

    builder.Entity<CompanyRole>(companyRole =>
    {
      companyRole
        .HasOne(v => v.Company)
        .WithMany(v => v.Roles)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);
    });

    builder.Entity<CompanyAccessInvitation>(companyAccessInvitation =>
    {
      companyAccessInvitation.Property(v => v.Email).HasColumnType("citext").HasMaxLength(64);

      companyAccessInvitation.HasIndex(v => new { v.Email, v.CompanyId }).IsUnique();

      companyAccessInvitation.Property(v => v.Status).HasConversion<string>();

      companyAccessInvitation
        .HasOne(v => v.Company)
        .WithMany(v => v.AccessInvitations)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);

      companyAccessInvitation
        .HasOne(v => v.Role)
        .WithMany()
        .HasForeignKey(v => v.RoleId)
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
