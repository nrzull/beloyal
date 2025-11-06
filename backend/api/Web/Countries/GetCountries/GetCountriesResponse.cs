namespace Beloyal.API.Web.Countries.GetCountries;

public abstract record GetCountriesResponse
{
  public sealed record Ok(List<string> Data) : GetCountriesResponse;
}
