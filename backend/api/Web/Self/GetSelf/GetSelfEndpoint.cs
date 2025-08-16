using Beloyal.API.Admin;
using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.GetSelf;

public class GetSelfEndpoint(AdminContext adminContext, CompanyContext companyContext)
{
  public async Task<GetSelfResponse> Handle(User user, AccessToken accessToken)
  {
    var adminAccess = await adminContext.GetAccess(UserId.From(user));

    var companyAccess = accessToken?.CompanyAccessId is Guid g
      ? await companyContext.GetAccess(UserId.From(user), CompanyAccessId.From(g))
      : (await companyContext.GetCompanyAccesses(UserId.From(user))).FirstOrDefault();

    var companyAccesses = await companyContext.GetCompanyAccesses(UserId.From(user));

    return new GetSelfResponse.Ok(
      User: UserDTO.From(user),
      AdminAccess: adminAccess is { } ? AdminAccessDTO.From(adminAccess) : null,
      CompanyAccess: companyAccess is { } ? CompanyAccessDTO.From(companyAccess) : null,
      CompanyAccesses: companyAccesses.Select(CompanyAccessDTO.From).ToList()
    );
  }
}
