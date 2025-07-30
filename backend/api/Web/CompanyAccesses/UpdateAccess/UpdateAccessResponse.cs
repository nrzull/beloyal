using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.UpdateAccess;

public abstract record UpdateAccessResponse
{
  public record Ok(CompanyAccessDTO Data) : UpdateAccessResponse;
}
