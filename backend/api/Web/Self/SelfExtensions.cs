using Beloyal.API.Web.Self.GetSelf;

namespace Beloyal.API.Web.Self;

public static class SelfExtensions
{
  public static void UseSelfEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetSelfEndpoint>();
  }
}
