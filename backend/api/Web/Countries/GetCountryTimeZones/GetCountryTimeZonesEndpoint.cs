using Beloyal.API.Countries;

namespace Beloyal.API.Web.Countries.GetCountryTimeZones;

public class GetCountryTimeZonesEndpoint(CountryContext countryContext)
{
  public GetCountryTimeZonesResponse.Ok Handle(CountryCode cc) =>
    new GetCountryTimeZonesResponse.Ok(countryContext.GetCountryTimeZones(cc));
}
