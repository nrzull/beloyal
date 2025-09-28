using Beloyal.API.Web.Company.GetBonusOptions;
using Beloyal.API.Web.Company.GetPublicLinks;
using Beloyal.API.Web.Company.SetBonusOptions;

namespace Beloyal.API.Web.Company;

public static class CompanyExtensions
{
  public static void UseCompanyEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetBonusOptionsEndpoint>();
    builder.Services.AddScoped<SetBonusOptionsEndpoint>();
    builder.Services.AddScoped<GetPublicLinksEndpoint>();
  }
}
