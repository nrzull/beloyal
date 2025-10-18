using Beloyal.API.Companies;

namespace Beloyal.API.Web.Company.UpdateCompany;

public class UpdateCompanyEndpoint(CompanyContext companyContext)
{
  public async Task<UpdateCompanyResponse> Handle(
    CompanyAccess companyAccess,
    UpdateCompanyRequest req
  )
  {
    var company = await companyContext.GetCompany(CompanyId.From(companyAccess));

    await companyContext.UpdateCompany(
      company,
      new(req.Tin),
      req.Title,
      TimeZoneInfo.FindSystemTimeZoneById(req.TimeZone)
    );

    return new UpdateCompanyResponse.Ok(CompanyDTO.From(company));
  }
}
