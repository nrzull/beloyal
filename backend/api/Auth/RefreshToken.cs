namespace Beloyal.API.Auth;

public record RefreshToken(Guid UserId, DateTimeOffset ExpiredAt);
