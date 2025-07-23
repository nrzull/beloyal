namespace Beloyal.API.Auth;

public record DecodedToken(Guid UserId, DateTimeOffset ExpiredAt);
