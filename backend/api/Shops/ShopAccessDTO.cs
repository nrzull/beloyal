using Beloyal.API.Users;

namespace Beloyal.API.Shops;

public record ShopAccessDTO(
  Guid Id,
  string? Phone,
  Guid CompanyAccessId,
  DateTimeOffset CreatedAt,
  string? FirstName,
  string? LastName,
  string? MiddleName
)
{
  public static ShopAccessDTO From(ShopAccess v) =>
    new(
      Id: v.Id,
      Phone: v.CompanyAccess?.User?.Phone,
      CreatedAt: v.CreatedAt,
      CompanyAccessId: v.CompanyAccessId,
      FirstName: v.CompanyAccess?.User?.FirstName,
      LastName: v.CompanyAccess?.User?.LastName,
      MiddleName: v.CompanyAccess?.User?.MiddleName
    );
};
