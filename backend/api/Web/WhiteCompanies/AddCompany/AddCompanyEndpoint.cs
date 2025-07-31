using Beloyal.API.Companies;

namespace Beloyal.API.Web.WhiteCompanies.AddCompany;

public class AddCompanyEndpoint(CompanyContext companyContext)
{
  public async Task<AddCompanyResponse> Handle(AddCompanyRequest req)
  {
    var company = await companyContext.CreateWhiteCompany(
      new(req.Tin),
      req.ExpiredAt.ToUniversalTime()
    );

    return new AddCompanyResponse.Ok(WhiteCompanyDTO.From(company));
  }
}
