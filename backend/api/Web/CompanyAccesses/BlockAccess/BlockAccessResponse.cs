namespace Beloyal.API.Web.CompanyAccesses.BlockAccess;

public abstract record BlockAccessResponse
{
  public record Ok : BlockAccessResponse;
}
