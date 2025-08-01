using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.CreateShopAccess;

public class CreateShopAccessEndpoint(CompanyContext companyContext, ShopContext shopContext)
{
  public async Task<CreateShopAccessResponse> Handle(
    CompanyAccess access,
    CreateShopAccessRequest req
  )
  {
    var shop = await shopContext.GetShop(CompanyId.From(access), ShopId.From(req.ShopId));
    if (shop is null)
    {
      throw new Exception("invalid shop");
    }

    var companyAccess = await companyContext.GetAccess(CompanyAccessId.From(req.CompanyAccessId));
    if (companyAccess is null || companyAccess.CompanyId != access.CompanyId)
    {
      throw new Exception("invalid company access");
    }

    var shopAccess = await shopContext.CreateAccess(shop, companyAccess);

    return new CreateShopAccessResponse.Ok(ShopAccessDTO.From(shopAccess));
  }
}
