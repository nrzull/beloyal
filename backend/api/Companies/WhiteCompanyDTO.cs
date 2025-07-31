namespace Beloyal.API.Companies;

public record WhiteCompanyDTO(
  Guid Id,
  string Tin,
  DateTimeOffset CreatedAt,
  DateTimeOffset ExpiredAt
)
{
  public static WhiteCompanyDTO From(WhiteCompany v) =>
    new(Id: v.Id, Tin: v.Tin, ExpiredAt: v.ExpiredAt, CreatedAt: v.CreatedAt);
};
