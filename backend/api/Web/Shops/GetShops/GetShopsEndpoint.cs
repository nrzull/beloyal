using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.GetShops;

public class GetShopsEndpoint(ShopContext shopContext)
{
  public async Task<GetShopsResponse> Handle(CompanyAccess access)
  {
    var shops = await shopContext.GetShops(CompanyId.From(access.CompanyId));

    return new GetShopsResponse.Ok(shops.Select(ShopDTO.From).ToList());
  }
}
