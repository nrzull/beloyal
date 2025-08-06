using Beloyal.API.Companies;
using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.CreateCompany;

public class CreateCompanyEndpoint(CompanyContext companyContext)
{
  public async Task<CreateCompanyResponse> Handle(
    User user,
    Tin tin,
    Percent cashbackPercent,
    Percent cashbackThreshold
  )
  {
    await companyContext.CreateCompany(user, tin, cashbackPercent, cashbackThreshold);
    return new CreateCompanyResponse.Ok();
  }
}
