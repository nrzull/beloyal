namespace Beloyal.API.Web.Countries.GetCountryTimeZones;

public abstract record GetCountryTimeZonesResponse
{
  public sealed record Ok(List<string> Data) : GetCountryTimeZonesResponse;
}
