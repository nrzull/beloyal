namespace Beloyal.API.Web.Company.GetPublicLinks;

public abstract record GetPublicLinksResponse
{
  public record Ok(List<OkData> Data) : GetPublicLinksResponse;

  public record OkData(string Type, string Url);
}
