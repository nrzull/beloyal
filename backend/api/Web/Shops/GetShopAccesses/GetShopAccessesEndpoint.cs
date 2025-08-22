using Beloyal.API.Companies;
using Beloyal.API.Shops;

namespace Beloyal.API.Web.Shops.GetShopAccesses;

public class GetShopAccessesEndpoint(ShopContext shopContext)
{
  public async Task<GetShopAccessesResponse> Handle(CompanyAccess access, ShopId shopId)
  {
    var accesses = await shopContext.GetAccesses(CompanyId.From(access.CompanyId), shopId);

    return new GetShopAccessesResponse.Ok(accesses.Select(ShopAccessDTO.From).ToList());
  }
}
