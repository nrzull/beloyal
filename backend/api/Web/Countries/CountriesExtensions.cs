using Beloyal.API.Web.Countries.GetCountries;
using Beloyal.API.Web.Countries.GetCountryTimeZones;

namespace Beloyal.API.Web.Countries;

public static class CountriesExtensions
{
  public static void UseCountriesEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetCountriesEndpoint>();
    builder.Services.AddScoped<GetCountryTimeZonesEndpoint>();
  }
}
