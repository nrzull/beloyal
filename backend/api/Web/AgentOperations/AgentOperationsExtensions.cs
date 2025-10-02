using Beloyal.API.Web.AgentOperations.GetOperations;

namespace Beloyal.API.Web.AgentOperations;

public static class AgentOperationsExtensions
{
  public static void UseAgentOperationsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetOperationsEndpoint>();
  }
}
