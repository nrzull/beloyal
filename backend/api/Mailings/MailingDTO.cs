namespace Beloyal.API.Mailings;

public record MailingDTO(Guid Id, DateTimeOffset CreatedAt, int Total, int Done, int Cancelled)
{
  public static MailingDTO From(Mailing v) =>
    new(
      Id: v.Id,
      CreatedAt: v.CreatedAt,
      Total: v.Units.Count,
      Done: v.Units.Count(vv => vv.Status == MailingUnitStatus.Done),
      Cancelled: v.Units.Count(vv => vv.Status == MailingUnitStatus.Cancelled)
    );
};
