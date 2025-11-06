using Beloyal.API.Countries;

namespace Beloyal.API.Web.Countries.GetCountries;

public class GetCountriesEndpoint
{
  public GetCountriesResponse.Ok Handle() =>
    new GetCountriesResponse.Ok(Enum.GetNames<CountryCode>().ToList());
}
