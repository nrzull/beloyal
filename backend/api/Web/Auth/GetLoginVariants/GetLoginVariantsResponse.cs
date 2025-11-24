namespace Beloyal.API.Web.Auth.GetLoginVariants;

public abstract record GetLoginVariantsResponse
{
  public record Ok(List<OkData> Data) : GetLoginVariantsResponse;

  public record OkData(string Type, string Value);
}
