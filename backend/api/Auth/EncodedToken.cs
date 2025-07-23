namespace Beloyal.API.Auth;

public record EncodedToken(string Token, DateTimeOffset ExpiredAt);
