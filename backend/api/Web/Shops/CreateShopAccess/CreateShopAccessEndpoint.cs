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
    var company = await companyContext.GetCompany(access);
    var shop = await shopContext.GetShop(company, req.ShopId);
    if (shop is null)
    {
      throw new Exception("invalid shop");
    }

    var companyAccess = await companyContext.GetAccess(req.CompanyAccessId);
    if (companyAccess is null || companyAccess.CompanyId != access.CompanyId)
    {
      throw new Exception("invalid company access");
    }

    var shopAccess = await shopContext.CreateAccess(shop, companyAccess);

    return new CreateShopAccessResponse.Ok(ShopAccessDTO.From(shopAccess));
  }
}
