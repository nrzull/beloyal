using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.GetAccess;

public abstract record GetAccessResponse
{
  public record NotFound : GetAccessResponse;

  public record Ok(CompanyAccessDTO Data) : GetAccessResponse;
}
