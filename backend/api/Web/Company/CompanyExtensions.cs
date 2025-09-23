using Beloyal.API.Web.Company.GetCashback;
using Beloyal.API.Web.Company.GetPublicLinks;
using Beloyal.API.Web.Company.SetCashback;

namespace Beloyal.API.Web.Company;

public static class CompanyExtensions
{
  public static void UseCompanyEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetCashbackEndpoint>();
    builder.Services.AddScoped<SetCashbackEndpoint>();
    builder.Services.AddScoped<GetPublicLinksEndpoint>();
  }
}
