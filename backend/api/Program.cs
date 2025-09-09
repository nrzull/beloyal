using Beloyal.API.Admin;
using Beloyal.API.Auth;
using Beloyal.API.Bonuses;
using Beloyal.API.Chats;
using Beloyal.API.Companies;
using Beloyal.API.Customers;
using Beloyal.API.Mailings;
using Beloyal.API.Messengers;
using Beloyal.API.Shops;
using Beloyal.API.Users;
using Beloyal.API.Web.AdminAccesses;
using Beloyal.API.Web.AdminRoles;
using Beloyal.API.Web.AgentSelf;
using Beloyal.API.Web.Auth;
using Beloyal.API.Web.Chats;
using Beloyal.API.Web.CompanyAccesses;
using Beloyal.API.Web.CompanyAccessInvitations;
using Beloyal.API.Web.CompanyRoles;
using Beloyal.API.Web.CustomerSelf;
using Beloyal.API.Web.Mailings;
using Beloyal.API.Web.Messengers;
using Beloyal.API.Web.Self;
using Beloyal.API.Web.Shops;
using Beloyal.API.Web.WhiteCompanies;

namespace Beloyal.API;

public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.SetupNpgsql();
    builder.SetupHttp();
    builder.SetupCors();
    builder.Services.AddControllers();

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

    builder.UseAuthEndpoints();
    builder.UseSelfEndpoints();
    builder.UseAdminRolesEndpoints();
    builder.UseAdminAccessesEndpoints();
    builder.UseCompanyRolesEndpoints();
    builder.UseCompanyAccessesEndpoints();
    builder.UseCompanyAccessInvitationsEndpoints();
    builder.UseWhiteCompaniesEndpoints();
    builder.UseShopsEndpoints();
    builder.UseMessengersEndpoints();
    builder.UseMailingsEndpoints();
    builder.UseChatsEndpoints();
    builder.UseCustomerSelfEndpoints();
    builder.UseAgentSelfEndpoints();

    builder.UseTgBotHandlers();

    var app = builder.Build();

    app.UseCustomCors();

    if (app.Environment.IsProduction())
    {
      app.UseHttpsRedirection();
    }

    app.UseAuthorization();
    app.MapControllers();

    await using var scope = app.Services.CreateAsyncScope();
    var tgContext = scope.ServiceProvider.GetRequiredService<TgContext>();

    await tgContext.PrepareClients();

    app.Run();
  }
}
