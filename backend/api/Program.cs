using Beloyal.API.Admin;
using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Beloyal.API.Users;

namespace Beloyal.API;

public class Program
{
  public static void Main(string[] args)
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

    var app = builder.Build();

    app.UseCustomCors();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
  }
}
