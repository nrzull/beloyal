namespace Beloyal.API.Shops;

public record ShopDTO(Guid Id, string Name, DateTimeOffset CreatedAt, Guid CompanyId)
{
  public static ShopDTO From(Shop v) =>
    new(Id: v.Id, Name: v.Name, CreatedAt: v.CreatedAt, CompanyId: v.CompanyId);
};
