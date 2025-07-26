using Beloyal.API.Web.Auth.Confirm;
using Beloyal.API.Web.Auth.Login;

namespace Beloyal.API.Web.Auth;

public static class AuthExtensions
{
  public static void UseAuthEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<LoginEndpoint>();
    builder.Services.AddScoped<ConfirmEndpoint>();
  }
}
