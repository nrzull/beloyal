using Beloyal.API.Admin;
using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.GetSelf;

public class GetSelfEndpoint(AdminContext adminContext, CompanyContext companyContext)
{
  public async Task<GetSelfResponse> Handle(User user, AccessToken accessToken)
  {
    var adminAccess = await adminContext.GetAccess(user);

    var companyAccess = accessToken.CompanyAccessId is Guid id
      ? await companyContext.GetAccess(user, id)
      : null;

    var companyAccesses = await companyContext.GetCompanyAccesses(user);

    return new GetSelfResponse.Ok(
      User: UserDTO.From(user),
      AdminAccess: adminAccess is { } ? AdminAccessDTO.From(adminAccess) : null,
      CompanyAccess: companyAccess is { } ? CompanyAccessDTO.From(companyAccess) : null,
      CompanyAccesses: companyAccesses.Select(CompanyAccessDTO.From).ToList()
    );
  }
}
