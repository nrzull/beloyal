using Beloyal.API.Admin;
using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Beloyal.API.Types;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Auth.Confirm;

public class ConfirmEndpoint(
  AdminContext adminContext,
  AuthContext authContext,
  UserContext userContext,
  CompanyContext companyContext,
  ShopContext shopContext
)
{
  public async Task<ConfirmResponse> Handle(Phone phone, string code, AccessToken? previousToken)
  {
    var otp = await authContext.DrainOtp(phone, code);
    if (otp is null)
    {
      throw new Exception("invalid otp");
    }

    var user = await userContext.EnsureUser(phone);
    await userContext.VerifyUser(user);

    var adminAccess = await adminContext.GetAccess(UserId.From(user));

    var companyAccess =
      previousToken?.CompanyAccessId is Guid g
        ? await companyContext.GetAccess(UserId.From(user), CompanyAccessId.From(g))
      : adminAccess is { } ? null
      : (await companyContext.GetCompanyAccesses(UserId.From(user))).FirstOrDefault();

    var shopAccess =
      companyAccess is null ? null
      : previousToken?.ShopAccessId is Guid shopAccessId
        ? await shopContext.GetAccess(ShopAccessId.From(shopAccessId))
      : null;

    if (shopAccess?.CompanyAccessId != companyAccess?.Id)
    {
      shopAccess = null;
    }

    var (t1, t2) = await authContext.IssueTokens(
      UserId.From(user),
      companyAccess is { } ? CompanyAccessId.From(companyAccess) : null,
      shopAccess is { } ? ShopAccessId.From(shopAccess) : null
    );

    return new ConfirmResponse.Ok(t1, t2);
  }
}
