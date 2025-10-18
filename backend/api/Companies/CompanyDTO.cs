namespace Beloyal.API.Companies;

public record CompanyDTO(
  Guid Id,
  DateTimeOffset CreatedAt,
  string Tin,
  string Title,
  string TimeZone
)
{
  public static CompanyDTO From(Company v) =>
    new(Id: v.Id, Tin: v.Tin, CreatedAt: v.CreatedAt, Title: v.Title, TimeZone: v.TimeZone.Id);
};
