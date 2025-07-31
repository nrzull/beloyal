using Beloyal.API.Web.WhiteCompanies.AddCompany;

namespace Beloyal.API.Web.WhiteCompanies;

public static class WhiteCompaniesExtensions
{
  public static void UseWhiteCompaniesEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<AddCompanyEndpoint>();
  }
}
