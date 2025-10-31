using Beloyal.API.Admin;
using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.GetSelf;

public class GetSelfEndpoint(
  AdminContext adminContext,
  CompanyContext companyContext,
  AuthContext authContext
)
{
  public async Task<(EncodedAccessToken?, EncodedRefreshToken?, GetSelfResponse)> Handle(
    User user,
    AccessToken accessToken
  )
  {
    var adminAccess = await adminContext.GetAccess(UserId.From(user));

    var companyAccess = accessToken.CompanyAccessId is Guid g
      ? await companyContext.GetAccess(UserId.From(user), CompanyAccessId.From(g))
      : null;

    var companyAccesses = await companyContext.GetCompanyAccesses(UserId.From(user));

    (EncodedAccessToken?, EncodedRefreshToken?) newTokens = default;
    if (companyAccess is null)
    {
      companyAccess = adminAccess is { }
        ? null
        : (await companyContext.GetCompanyAccesses(UserId.From(user))).FirstOrDefault();

      newTokens = await authContext.IssueTokens(user, companyAccess, null);
    }

    return (
      newTokens.Item1,
      newTokens.Item2,
      new GetSelfResponse.Ok(
        User: UserDTO.From(user),
        AdminAccess: adminAccess is { } ? AdminAccessDTO.From(adminAccess) : null,
        CompanyAccess: companyAccess is { } ? CompanyAccessDTO.From(companyAccess) : null,
        CompanyAccesses: companyAccesses.Select(CompanyAccessDTO.From).ToList()
      )
    );
  }
}
