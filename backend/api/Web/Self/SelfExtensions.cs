using Beloyal.API.Web.Self.CreateCompany;
using Beloyal.API.Web.Self.GetSelf;
using Beloyal.API.Web.Self.UpdateSelf;

namespace Beloyal.API.Web.Self;

public static class SelfExtensions
{
  public static void UseSelfEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetSelfEndpoint>();
    builder.Services.AddScoped<CreateCompanyEndpoint>();
    builder.Services.AddScoped<UpdateSelfEndpoint>();
  }
}
