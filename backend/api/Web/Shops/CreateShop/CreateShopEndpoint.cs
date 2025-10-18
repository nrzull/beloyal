using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.CreateShop;

public class CreateShopEndpoint(CompanyContext companyContext, ShopContext shopContext)
{
  public async Task<CreateShopResponse> Handle(CompanyAccess access, CreateShopRequest req)
  {
    var company = await companyContext.GetCompany(CompanyId.From(access));
    var shop = await shopContext.CreateShop(
      company,
      req.Name,
      req.TimeZone is { } ? TimeZoneInfo.FindSystemTimeZoneById(req.TimeZone) : null
    );

    return new CreateShopResponse.Ok(ShopDTO.From(shop));
  }
}
