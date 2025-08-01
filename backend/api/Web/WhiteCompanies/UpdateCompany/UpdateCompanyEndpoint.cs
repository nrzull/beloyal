using Beloyal.API.Companies;

namespace Beloyal.API.Web.WhiteCompanies.UpdateCompany;

public class UpdateCompanyEndpoint(CompanyContext companyContext)
{
  public async Task<UpdateCompanyResponse> Handle(UpdateCompanyRequest req)
  {
    var whiteCompany = await companyContext.GetWhiteCompany(WhiteCompanyId.From(req.Id));
    if (whiteCompany is null)
    {
      throw new Exception("invalid white company");
    }

    await companyContext.UpdateWhiteCompany(whiteCompany, req.ExpiredAt.ToUniversalTime());

    return new UpdateCompanyResponse.Ok(WhiteCompanyDTO.From(whiteCompany));
  }
}
