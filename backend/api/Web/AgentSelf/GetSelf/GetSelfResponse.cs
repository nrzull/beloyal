using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Beloyal.API.Users;

namespace Beloyal.API.Web.AgentSelf.GetSelf;

public abstract record GetSelfResponse
{
  public record Ok(
    UserDTO User,
    ShopDTO? Shop,
    List<ShopDTO> Shops,
    CompanyAccessDTO? CompanyAccess,
    List<CompanyAccessDTO> CompanyAccesses
  ) : GetSelfResponse;
}
