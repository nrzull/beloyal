using Beloyal.API.Countries;

namespace Beloyal.API.Web.Countries.GetCountryLangs;

public class GetCountryLangsEndpoint(CountryContext countryContext)
{
  public GetCountryLangsResponse.Ok Handle(CountryCode cc) =>
    new GetCountryLangsResponse.Ok(
      countryContext.GetCountryLangs(cc).Select(v => v.ToString()).ToList()
    );
}
