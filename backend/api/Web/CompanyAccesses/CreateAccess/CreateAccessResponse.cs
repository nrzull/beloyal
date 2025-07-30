using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.CreateAccess;

public abstract record CreateAccessResponse
{
  public record Ok(CompanyAccessDTO Data) : CreateAccessResponse;
}
