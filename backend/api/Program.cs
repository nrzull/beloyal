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
using Beloyal.API.Shops;
using Beloyal.API.Users;
using Beloyal.API.Web.AdminAccesses;
using Beloyal.API.Web.AdminRoles;
using Beloyal.API.Web.AgentCustomers;
using Beloyal.API.Web.AgentOperations;
using Beloyal.API.Web.AgentSelf;
using Beloyal.API.Web.Analytics;
using Beloyal.API.Web.Auth;
using Beloyal.API.Web.Chats;
using Beloyal.API.Web.Company;
using Beloyal.API.Web.CompanyAccesses;
using Beloyal.API.Web.CompanyRoles;
using Beloyal.API.Web.CustomerOperations;
using Beloyal.API.Web.Customers;
using Beloyal.API.Web.CustomerSelf;
using Beloyal.API.Web.Handlers;
using Beloyal.API.Web.Levels;
using Beloyal.API.Web.Mailings;
using Beloyal.API.Web.Messengers;
using Beloyal.API.Web.Middlewares;
using Beloyal.API.Web.Operations;
using Beloyal.API.Web.Promotions;
using Beloyal.API.Web.Self;
using Beloyal.API.Web.Shops;
using Beloyal.API.Web.WhiteCompanies;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Scalar.AspNetCore;

namespace Beloyal.API;

public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();

    builder
      .Services.AddOpenTelemetry()
      .WithLogging(
        opts =>
        {
          opts.SetResourceBuilder(
            ResourceBuilder
              .CreateEmpty()
              .AddService(builder.Environment.ApplicationName)
              .AddAttributes(
                new Dictionary<string, object>()
                {
                  ["deployment.Environment"] = builder.Environment.EnvironmentName,
                }
              )
          );

          if (builder.Environment.IsDevelopment())
          {
            opts.AddConsoleExporter();
          }

          opts.AddOtlpExporter(v =>
          {
            v.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            v.Endpoint = new Uri(Environment.GetEnvironmentVariable("SEQ_INGEST_URL")!);
            v.Headers = $"X-Seq-ApiKey={Environment.GetEnvironmentVariable("SEQ_INGEST_TOKEN")!}";
          });
        },
        opts =>
        {
          opts.IncludeScopes = true;
          opts.IncludeFormattedMessage = true;
        }
      );

    builder.SetupNpgsql();
    builder.SetupHttp();
    builder.SetupCors();
    builder.SetupOpenApi();
    builder.SetupS3();
    builder.Services.AddControllers();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddTransient<HubAuthMiddleware>();

    builder.Services.AddScoped<AuthContext>();
    builder.Services.AddScoped<UserContext>();
    builder.Services.AddScoped<AdminContext>();
    builder.Services.AddScoped<CompanyContext>();
    builder.Services.AddScoped<ShopContext>();
    builder.Services.AddScoped<TgContext>();
    builder.Services.AddScoped<CustomerContext>();
    builder.Services.AddScoped<BonusContext>();
    builder.Services.AddScoped<ChatContext>();
    builder.Services.AddScoped<MailingContext>();
    builder.Services.AddScoped<PromotionContext>();
    builder.Services.AddScoped<BoosterContext>();
    builder.Services.AddScoped<LevelContext>();
    builder.Services.AddHostedService<BonusLifetimeBackgroundService>();
    builder.Services.AddHostedService<BonusSchedulingBackgroundService>();
    builder.Services.AddHostedService<LevelWorker>();
    builder.Services.AddSignalR();

    builder.UseAuthEndpoints();
    builder.UseSelfEndpoints();
    builder.UseAdminRolesEndpoints();
    builder.UseAdminAccessesEndpoints();
    builder.UseCompanyRolesEndpoints();
    builder.UseCompanyAccessesEndpoints();
    builder.UseWhiteCompaniesEndpoints();
    builder.UseShopsEndpoints();
    builder.UseMessengersEndpoints();
    builder.UseMailingsEndpoints();
    builder.UseChatsEndpoints();
    builder.UseCustomerSelfEndpoints();
    builder.UseAgentSelfEndpoints();
    builder.UseAgentCustomersEndpoints();
    builder.UseCompanyEndpoints();
    builder.UseOperationsEndpoints();
    builder.UseAgentOperationsEndpoints();
    builder.UseCustomerOperationsEndpoints();
    builder.UsePromotionsEndpoints();
    builder.UseCustomersEndpoints();
    builder.UseLevelsEndpoints();
    builder.UseAnalyticsEndpoints();

    builder.UseTgBotHandlers();

    var app = builder.Build();

    app.UseCustomCors();

    if (app.Environment.IsProduction())
    {
      app.UseHttpsRedirection();
    }
    else
    {
      app.MapOpenApi();
      app.MapScalarApiReference("/api/scalar/v1");
    }

    app.UseMiddleware<HubAuthMiddleware>();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHub<ApplicationHub>("/api/ws");
    app.UseExceptionHandler();
    app.UseStatusCodePages();

    AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
    {
      var logger = app.Services.GetRequiredService<ILogger<Program>>();
      var exception = (Exception)e.ExceptionObject;
      logger.LogError(exception, "Uncaught error: {UncaughtMessage}", exception.Message);
    };

    await using var scope = app.Services.CreateAsyncScope();
    var tgContext = scope.ServiceProvider.GetRequiredService<TgContext>();

    await tgContext.PrepareClients();

    app.Run();
  }
}
