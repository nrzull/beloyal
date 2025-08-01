namespace Beloyal.API.Shops;

public record ShopAccessDTO(Guid Id, string? UserEmail)
{
  public static ShopAccessDTO From(ShopAccess v) =>
    new(Id: v.Id, UserEmail: v.CompanyAccess?.User?.Email);
};
