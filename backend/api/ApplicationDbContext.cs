using System.Globalization;
using System.Text.Json;
using Beloyal.API.Admin;
using Beloyal.API.Auth;
using Beloyal.API.Bonuses;
using Beloyal.API.Boosters;
using Beloyal.API.Chats;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Levels;
using Beloyal.API.Mailings;
using Beloyal.API.Messengers;
using Beloyal.API.Promotions;
using Beloyal.API.Promotions.Variants;
using Beloyal.API.Seeds;
using Beloyal.API.Shops;
using Beloyal.API.Users;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API;

public class ApplicationDbContext(DbContextOptions opts) : DbContext(opts)
{
  public DbSet<AuthOtp> AuthOtps { get; set; }
  public DbSet<PersistedToken> PersistedTokens { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<AdminAccess> AdminAccesses { get; set; }
  public DbSet<AdminRole> AdminRoles { get; set; }
  public DbSet<WhiteCompany> WhiteCompanies { get; set; }
  public DbSet<Company> Companies { get; set; }
  public DbSet<CompanyAccess> CompanyAccesses { get; set; }
  public DbSet<CompanyRole> CompanyRoles { get; set; }
  public DbSet<Shop> Shops { get; set; }
  public DbSet<ShopAccess> ShopAccesses { get; set; }
  public DbSet<Customer> Customers { get; set; }
  public DbSet<MaxBot> MaxBots { get; set; }
  public DbSet<TgBot> TgBots { get; set; }
  public DbSet<Bonus> Bonuses { get; set; }
  public DbSet<Chat> Chats { get; set; }
  public DbSet<ChatMessage> ChatMessages { get; set; }
  public DbSet<Mailing> Mailings { get; set; }
  public DbSet<MailingUnit> MailingUnits { get; set; }
  public DbSet<Promotion> Promotions { get; set; }
  public DbSet<ShopPromotion> ShopPromotions { get; set; }
  public DbSet<Booster> Boosters { get; set; }
  public DbSet<Level> Levels { get; set; }

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
      user.Property(v => v.Phone).HasMaxLength(16);
      user.HasIndex(v => v.Phone).IsUnique();

      user.Property(v => v.TgUserId).HasMaxLength(32);
      user.HasIndex(v => v.TgUserId).IsUnique();

      user.Property(v => v.MaxUserId).HasMaxLength(32);
      user.HasIndex(v => v.MaxUserId).IsUnique();
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

      whiteCompany
        .HasOne(v => v.Company)
        .WithOne()
        .HasForeignKey<WhiteCompany>(v => v.CompanyId)
        .HasPrincipalKey<Company>(v => v.Id);
    });

    builder.Entity<Company>(company =>
    {
      company.Property(v => v.Tin).HasMaxLength(12);
      company.HasIndex(v => v.Tin).IsUnique();
      company.OwnsOne(v => v.BonusFrequency);
      company
        .Property(v => v.TimeZone)
        .HasConversion(v => v.Id, v => TimeZoneInfo.FindSystemTimeZoneById(v));

      company
        .HasOne(v => v.Owner)
        .WithMany()
        .HasForeignKey(v => v.OwnerId)
        .HasPrincipalKey(v => v.Id);
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

    builder.Entity<Shop>(shop =>
    {
      shop.HasOne(v => v.Company)
        .WithMany(v => v.Shops)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);

      shop.Property(v => v.TimeZone)
        .HasConversion(
          v => v == null ? null : v.Id,
          v => v == null ? null : TimeZoneInfo.FindSystemTimeZoneById(v)
        );
    });

    builder.Entity<ShopAccess>(shopAccess =>
    {
      shopAccess
        .HasOne(v => v.CompanyAccess)
        .WithMany(v => v.ShopAccesses)
        .HasForeignKey(v => v.CompanyAccessId)
        .HasPrincipalKey(v => v.Id);

      shopAccess
        .HasOne(v => v.Shop)
        .WithMany(v => v.Accesses)
        .HasForeignKey(v => v.ShopId)
        .HasPrincipalKey(v => v.Id);

      shopAccess.HasIndex(v => new { v.CompanyAccessId, v.ShopId }).IsUnique();
    });

    builder.Entity<Customer>(customer =>
    {
      customer.HasIndex(v => new { v.UserId, v.CompanyId }).IsUnique();

      customer
        .HasOne(v => v.MaxBot)
        .WithMany(v => v.Customers)
        .HasForeignKey(v => v.MaxBotId)
        .HasPrincipalKey(v => v.Id);

      customer
        .HasOne(v => v.TgBot)
        .WithMany(v => v.Customers)
        .HasForeignKey(v => v.TgBotId)
        .HasPrincipalKey(v => v.Id);

      customer
        .HasOne(v => v.Company)
        .WithMany(v => v.Customers)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);

      customer
        .HasOne(v => v.User)
        .WithMany(v => v.Customers)
        .HasForeignKey(v => v.UserId)
        .HasPrincipalKey(v => v.Id);
    });

    builder.Entity<TgBot>(tgBot =>
    {
      tgBot
        .HasOne(v => v.Company)
        .WithOne(v => v.TgBot)
        .HasForeignKey<TgBot>(v => v.CompanyId)
        .HasPrincipalKey<Company>(v => v.Id);
    });

    builder.Entity<MaxBot>(maxBot =>
    {
      maxBot
        .HasOne(v => v.Company)
        .WithOne(v => v.MaxBot)
        .HasForeignKey<MaxBot>(v => v.CompanyId)
        .HasPrincipalKey<Company>(v => v.Id);
    });

    builder.Entity<Bonus>(bonus =>
    {
      bonus.Property(v => v.Type).HasConversion<string>();
      bonus.Property(v => v.Status).HasConversion<string>();

      bonus.OwnsOne(
        v => v.Promotion,
        (promotion) =>
        {
          promotion
            .Property(v => v.Variant)
            .HasConversion(
              v =>
                JsonSerializer.Serialize(v, BaseVariantJsonSerializerContext.Default.BaseVariant),
              (v) =>
                JsonSerializer.Deserialize(v, BaseVariantJsonSerializerContext.Default.BaseVariant)!
            );
        }
      );

      bonus
        .HasOne(v => v.Customer)
        .WithMany(v => v.Bonuses)
        .HasForeignKey(v => v.CustomerId)
        .HasPrincipalKey(v => v.Id);

      bonus
        .HasOne(v => v.Agent)
        .WithMany()
        .HasForeignKey(v => v.AgentId)
        .HasPrincipalKey(v => v.Id);

      bonus.HasOne(v => v.Shop).WithMany().HasForeignKey(v => v.ShopId).HasPrincipalKey(v => v.Id);

      bonus
        .HasOne(v => v.Booster)
        .WithOne(v => v.Bonus)
        .HasForeignKey<Bonus>(v => v.BoosterId)
        .HasPrincipalKey<Booster>(v => v.Id);
    });

    builder.Entity<Chat>(chat =>
    {
      chat.HasOne(v => v.Company)
        .WithMany(v => v.Chats)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);

      chat.HasOne(v => v.Customer)
        .WithMany()
        .HasForeignKey(v => v.CustomerId)
        .HasPrincipalKey(v => v.Id);
    });

    builder.Entity<ChatMessage>(chatMessage =>
    {
      chatMessage
        .HasOne(v => v.Customer)
        .WithMany()
        .HasForeignKey(v => v.CustomerId)
        .HasPrincipalKey(v => v.Id);

      chatMessage
        .HasOne(v => v.Agent)
        .WithMany()
        .HasForeignKey(v => v.AgentId)
        .HasPrincipalKey(v => v.Id);

      chatMessage
        .HasOne(v => v.Chat)
        .WithMany(v => v.Messages)
        .HasForeignKey(v => v.ChatId)
        .HasPrincipalKey(v => v.Id);

      chatMessage
        .HasOne(v => v.Company)
        .WithMany()
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);

      chatMessage
        .HasOne(v => v.MailingUnit)
        .WithOne(v => v.ChatMessage)
        .HasForeignKey<ChatMessage>(v => v.MailingUnitId)
        .HasPrincipalKey<MailingUnit>(v => v.Id);
    });

    builder.Entity<Mailing>(mailing =>
    {
      mailing
        .HasOne(v => v.Company)
        .WithMany(v => v.Mailings)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);
    });

    builder.Entity<MailingUnit>(mailingUnit =>
    {
      mailingUnit
        .HasOne(v => v.Mailing)
        .WithMany(v => v.Units)
        .HasForeignKey(v => v.MailingId)
        .HasPrincipalKey(v => v.Id);

      mailingUnit
        .HasOne(v => v.Customer)
        .WithMany()
        .HasForeignKey(v => v.CustomerId)
        .HasPrincipalKey(v => v.Id);

      mailingUnit.Property(v => v.Status).HasConversion<string>();
    });

    builder.Entity<Promotion>(promotion =>
    {
      promotion
        .HasOne(v => v.Company)
        .WithMany(v => v.Promotions)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);

      promotion.HasMany(v => v.Shops).WithMany(v => v.Promotions).UsingEntity<ShopPromotion>();

      promotion
        .Property(v => v.Variant)
        .HasConversion(
          v => JsonSerializer.Serialize(v, BaseVariantJsonSerializerContext.Default.BaseVariant),
          (v) =>
            JsonSerializer.Deserialize(v, BaseVariantJsonSerializerContext.Default.BaseVariant)!
        );
    });

    builder.Entity<Booster>(booster =>
    {
      booster
        .HasOne(v => v.Customer)
        .WithMany(v => v.Boosters)
        .HasForeignKey(v => v.CustomerId)
        .HasPrincipalKey(v => v.Id);
    });

    builder.Entity<Level>(level =>
    {
      level
        .HasOne(v => v.Company)
        .WithMany(v => v.Levels)
        .HasForeignKey(v => v.CompanyId)
        .HasPrincipalKey(v => v.Id);
    });

    builder.Entity<PersistedToken>(persistedToken =>
    {
      persistedToken.HasKey(v => v.UserId);
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
