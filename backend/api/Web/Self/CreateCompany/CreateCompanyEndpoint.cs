using Beloyal.API.Companies;
using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.CreateCompany;

public class CreateCompanyEndpoint(CompanyContext companyContext)
{
  public async Task<CreateCompanyResponse> Handle(User user, CreateCompanyRequest req)
  {
    await companyContext.CreateCompany(
      user,
      new Tin(req.Tin),
      new Percent(req.cashbackPercent),
      new Percent(req.cashbackThreshold)
    );

    return new CreateCompanyResponse.Ok();
  }
}
