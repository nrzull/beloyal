using Beloyal.API.Companies;

namespace Beloyal.API.Web.Company.UpdateCompany;

public abstract record UpdateCompanyResponse
{
  public record Ok(CompanyDTO Data) : UpdateCompanyResponse;
}
