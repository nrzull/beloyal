using Beloyal.API.Auth;
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

    var app = builder.Build();

    app.UseCustomCors();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
  }
}
