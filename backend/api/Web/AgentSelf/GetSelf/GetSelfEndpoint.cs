using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Beloyal.API.Users;

namespace Beloyal.API.Web.AgentSelf.GetSelf;

public class GetSelfEndpoint(
  CompanyContext companyContext,
  AuthContext authContext,
  ShopContext shopContext
)
{
  public async Task<(EncodedAccessToken?, EncodedRefreshToken?, GetSelfResponse.Ok)> Handle(
    User user,
    AccessToken accessToken
  )
  {
    var companyAccess = accessToken.CompanyAccessId is Guid g
      ? await companyContext.GetAccess(UserId.From(user), CompanyAccessId.From(g))
      : null;

    var shopAccess = accessToken.ShopAccessId is Guid sg
      ? await shopContext.GetAccess(ShopAccessId.From(sg))
      : null;

    if (shopAccess?.CompanyAccessId != companyAccess?.Id)
    {
      shopAccess = null;
    }

    bool isNewToken = false;
    if (companyAccess is null)
    {
      companyAccess = (await companyContext.GetCompanyAccesses(UserId.From(user))).FirstOrDefault();

      isNewToken = true;
    }

    var companyAccesses = await companyContext.GetCompanyAccesses(UserId.From(user));

    var shops = companyAccess is { }
      ? await shopContext.GetShops(CompanyAccessId.From(companyAccess))
      : [];

    var shop = shopAccess is { } sa ? await shopContext.GetShop(ShopAccessId.From(sa)) : null;

    var (t1, t2) = isNewToken
      ? await authContext.IssueTokens(
        UserId.From(user),
        companyAccess is { } ? CompanyAccessId.From(companyAccess) : null,
        null
      )
      : (null, null);

    return (
      t1,
      t2,
      new GetSelfResponse.Ok(
        User: UserDTO.From(user),
        Shop: shop is { } ? ShopDTO.From(shop) : null,
        Shops: shops.Select(ShopDTO.From).ToList(),
        CompanyAccess: companyAccess is { } ? CompanyAccessDTO.From(companyAccess) : null,
        CompanyAccesses: companyAccesses.Select(CompanyAccessDTO.From).ToList()
      )
    );
  }
}
