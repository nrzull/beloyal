namespace Beloyal.API.Shops;

public record ShopAccessDTO(Guid Id, string? Phone, Guid CompanyAccessId, DateTimeOffset CreatedAt)
{
  public static ShopAccessDTO From(ShopAccess v) =>
    new(
      Id: v.Id,
      Phone: v.CompanyAccess?.User?.Phone,
      CreatedAt: v.CreatedAt,
      CompanyAccessId: v.CompanyAccessId
    );
};
