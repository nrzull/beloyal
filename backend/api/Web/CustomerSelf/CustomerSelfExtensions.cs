using Beloyal.API.Web.CustomerSelf.GetCustomerSelf;

namespace Beloyal.API.Web.CustomerSelf;

public static class CustomerSelfExtensions
{
  public static void UseCustomerSelfEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetCustomerSelfEndpoint>();
  }
}
