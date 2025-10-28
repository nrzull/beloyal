using Beloyal.API.Web.Levels.CreateLevel;
using Beloyal.API.Web.Levels.DeleteLevel;
using Beloyal.API.Web.Levels.GetLevels;
using Beloyal.API.Web.Levels.UpdateLevel;

namespace Beloyal.API.Web.Levels;

public static class LevelsExtensions
{
  public static void UseLevelsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateLevelEndpoint>();
    builder.Services.AddScoped<GetLevelsEndpoint>();
    builder.Services.AddScoped<DeleteLevelEndpoint>();
    builder.Services.AddScoped<UpdateLevelEndpoint>();
  }
}
