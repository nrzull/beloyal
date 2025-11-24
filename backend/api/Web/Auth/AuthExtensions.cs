using Beloyal.API.Web.Auth.Confirm;
using Beloyal.API.Web.Auth.GetLoginVariants;
using Beloyal.API.Web.Auth.Login;
using Beloyal.API.Web.Auth.LoginAsTgCustomer;

namespace Beloyal.API.Web.Auth;

public static class AuthExtensions
{
  public static void UseAuthEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<LoginEndpoint>();
    builder.Services.AddScoped<ConfirmEndpoint>();
    builder.Services.AddScoped<LoginAsTgCustomerEndpoint>();
    builder.Services.AddScoped<GetLoginVariantsEndpoint>();
  }
}
