using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Users;

namespace Beloyal.API.Web.AgentSelf.SetCompanyAccess;

public class SetCompanyAccessEndpoint(CompanyContext companyContext, AuthContext authContext)
{
  public async Task<SetCompanyAccessResponse> Handle(User user, SetCompanyAccessRequest req)
  {
    var companyAccess = req.CompanyAccessId is { } id
      ? await companyContext.GetAccess(CompanyAccessId.From(id))
      : null;

    if (companyAccess is { } && companyAccess.UserId != user.Id)
    {
      throw new Exception("invalid company access");
    }

    var (t1, t2) = await authContext.IssueTokens(user, companyAccess, null);

    return new SetCompanyAccessResponse.Ok(t1, t2);
  }
}
