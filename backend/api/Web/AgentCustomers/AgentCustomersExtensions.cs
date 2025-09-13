using Beloyal.API.Web.AgentCustomers.AddBonus;
using Beloyal.API.Web.AgentCustomers.GetCustomerByPublicToken;
using Beloyal.API.Web.AgentCustomers.SubtractBonus;

namespace Beloyal.API.Web.AgentCustomers;

public static class AgentCustomersExtensions
{
  public static void UseAgentCustomersEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetCustomerByPublicTokenEndpoint>();
    builder.Services.AddScoped<AddBonusEndpoint>();
    builder.Services.AddScoped<SubtractBonusEndpoint>();
  }
}
