using Beloyal.API.Web.CustomerOperations.GetOperations;

namespace Beloyal.API.Web.CustomerOperations;

public static class CustomerOperationsExtensions
{
  public static void UseCustomerOperationsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetOperationsEndpoint>();
  }
}
