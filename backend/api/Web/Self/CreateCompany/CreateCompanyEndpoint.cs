using Beloyal.API.Companies;
using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.CreateCompany;

public class CreateCompanyEndpoint(CompanyContext companyContext)
{
  public async Task<CreateCompanyResponse> Handle(User user, Tin tin)
  {
    await companyContext.CreateCompany(user, tin);
    return new CreateCompanyResponse.Ok();
  }
}
