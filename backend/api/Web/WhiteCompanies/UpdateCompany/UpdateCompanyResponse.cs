using Beloyal.API.Companies;

namespace Beloyal.API.Web.WhiteCompanies.UpdateCompany;

public abstract record UpdateCompanyResponse
{
  public record Ok(WhiteCompanyDTO Data) : UpdateCompanyResponse;
}
