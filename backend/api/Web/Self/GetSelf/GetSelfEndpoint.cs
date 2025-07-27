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

    return new GetSelfResponse.Ok(
      UserDTO.From(user),
      adminAccess is { } ? AdminAccessDTO.From(adminAccess) : null,
      companyAccess is { } ? CompanyAccessDTO.From(companyAccess) : null
    );
  }
}
