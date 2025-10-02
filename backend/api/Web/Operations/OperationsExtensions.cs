using Beloyal.API.Web.Operations.CancelOperation;
using Beloyal.API.Web.Operations.GetOperations;

namespace Beloyal.API.Web.Operations;

public static class OperationsExtensions
{
  public static void UseOperationsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetOperationsEndpoint>();
    builder.Services.AddScoped<CancelOperationEndpoint>();
  }
}
