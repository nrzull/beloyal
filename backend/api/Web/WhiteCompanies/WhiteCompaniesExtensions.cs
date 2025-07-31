using Beloyal.API.Web.WhiteCompanies.AddCompany;
using Beloyal.API.Web.WhiteCompanies.UpdateCompany;

namespace Beloyal.API.Web.WhiteCompanies;

public static class WhiteCompaniesExtensions
{
  public static void UseWhiteCompaniesEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<AddCompanyEndpoint>();
    builder.Services.AddScoped<UpdateCompanyEndpoint>();
  }
}
