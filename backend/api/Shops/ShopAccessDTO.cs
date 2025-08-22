namespace Beloyal.API.Shops;

public record ShopAccessDTO(Guid Id, string? Email, Guid CompanyAccessId, DateTimeOffset CreatedAt)
{
  public static ShopAccessDTO From(ShopAccess v) =>
    new(
      Id: v.Id,
      Email: v.CompanyAccess?.User?.Email,
      CreatedAt: v.CreatedAt,
      CompanyAccessId: v.CompanyAccessId
    );
};
