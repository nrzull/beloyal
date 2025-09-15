namespace Beloyal.API.Auth;

public record CustomerAccessToken(
  Guid CustomerId,
  DateTimeOffset IssuedAt,
  DateTimeOffset ExpiredAt
);
