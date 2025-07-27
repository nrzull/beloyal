using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Auth.Confirm;

public class ConfirmEndpoint(
  AuthContext authContext,
  UserContext userContext,
  CompanyContext companyContext
)
{
  public async Task<ConfirmResponse> Handle(Email email, string code, AccessToken? previousToken)
  {
    var otp = await authContext.DrainEmailOTP(email, code);
    if (otp is null)
    {
      throw new Exception("invalid otp");
    }

    var user = await userContext.EnsureUser(email);

    var companyAccess = previousToken?.CompanyAccessId is Guid g
      ? await companyContext.GetAccess(user, g)
      : (await companyContext.GetCompanyAccesses(user)).FirstOrDefault();

    var token = authContext.EncodeAccessToken(user, companyAccess);

    return new ConfirmResponse.Ok(token);
  }
}
