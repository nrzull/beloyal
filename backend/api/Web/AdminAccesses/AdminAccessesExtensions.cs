using Beloyal.API.Web.AdminAccesses.CreateAccess;
using Beloyal.API.Web.AdminAccesses.RemoveAccess;
using Beloyal.API.Web.AdminAccesses.UpdateAccess;

namespace Beloyal.API.Web.AdminAccesses;

public static class AdminAccessesExtensions
{
  public static void UseAdminAccessesEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateAccessEndpoint>();
    builder.Services.AddScoped<UpdateAccessEndpoint>();
    builder.Services.AddScoped<RemoveAccessEndpoint>();
  }
}
