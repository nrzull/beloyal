using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Users;

namespace Beloyal.API.Web.AgentSelf.SetCompanyAccess;

public class SetCompanyAccessEndpoint(CompanyContext companyContext, AuthContext authContext)
{
  public async Task<SetCompanyAccessResponse> Handle(User user, SetCompanyAccessRequest req)
  {
    var companyAccess = await companyContext.GetAccess(CompanyAccessId.From(req.CompanyAccessId));
    if (companyAccess is null || companyAccess.UserId != user.Id)
    {
      throw new Exception("invalid company access");
    }

    var token = authContext.EncodeAccessToken(user, companyAccess, null);

    return new SetCompanyAccessResponse.Ok(token);
  }
}
