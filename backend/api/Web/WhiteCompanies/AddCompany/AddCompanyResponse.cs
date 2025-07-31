using Beloyal.API.Companies;

namespace Beloyal.API.Web.WhiteCompanies.AddCompany;

public abstract record AddCompanyResponse
{
  public record Ok(WhiteCompanyDTO Data) : AddCompanyResponse;
}
