namespace Beloyal.API.Web.Countries.GetCountryLangs;

public abstract record GetCountryLangsResponse
{
  public sealed record Ok(List<string> Data) : GetCountryLangsResponse;
}
