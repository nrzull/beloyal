namespace Beloyal.API.Auth;

public record EncodedRefreshToken(string Token, DateTimeOffset ExpiredAt);
