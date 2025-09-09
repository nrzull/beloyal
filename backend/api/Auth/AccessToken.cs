namespace Beloyal.API.Auth;

public record AccessToken(
  Guid UserId,
  Guid? CompanyAccessId,
  Guid? ShopAccessId,
  DateTimeOffset ExpiredAt
);
