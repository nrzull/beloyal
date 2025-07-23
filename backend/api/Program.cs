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

    var app = builder.Build();

    app.UseCustomCors();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
  }
}
