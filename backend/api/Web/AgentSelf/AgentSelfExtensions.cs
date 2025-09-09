using Beloyal.API.Web.AgentSelf.GetSelf;
using Beloyal.API.Web.AgentSelf.SetCompanyAccess;
using Beloyal.API.Web.AgentSelf.SetShopAccess;

namespace Beloyal.API.Web.AgentSelf;

public static class AgentSelfExtensions
{
  public static void UseAgentSelfEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetSelfEndpoint>();
    builder.Services.AddScoped<SetCompanyAccessEndpoint>();
    builder.Services.AddScoped<SetShopAccessEndpoint>();
  }
}
