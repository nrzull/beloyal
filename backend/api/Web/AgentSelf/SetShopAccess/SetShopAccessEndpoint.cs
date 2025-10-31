using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Beloyal.API.Users;

namespace Beloyal.API.Web.AgentSelf.SetShopAccess;

public class SetShopAccessEndpoint(
  CompanyContext companyContext,
  AuthContext authContext,
  ShopContext shopContext
)
{
  public async Task<SetShopAccessResponse> Handle(User user, SetShopAccessRequest req)
  {
    var companyAccess = await companyContext.GetAccess(CompanyAccessId.From(req.CompanyAccessId));
    if (companyAccess is null || companyAccess.UserId != user.Id)
    {
      throw new Exception("invalid company access");
    }

    var shopAccess = await shopContext.GetAccess(
      ShopId.From(req.ShopId),
      CompanyAccessId.From(req.CompanyAccessId)
    );
    if (shopAccess is null)
    {
      throw new Exception("invalid shop access");
    }

    var (t1, t2) = await authContext.IssueTokens(user, companyAccess, shopAccess);

    return new SetShopAccessResponse.Ok(t1, t2);
  }
}
