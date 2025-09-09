using Beloyal.API.Web.AgentSelf.GetSelf;

namespace Beloyal.API.Web.AgentSelf;

public static class AgentSelfExtensions
{
  public static void UseAgentSelfEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetSelfEndpoint>();
  }
}
