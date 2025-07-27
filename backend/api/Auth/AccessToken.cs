namespace Beloyal.API.Auth;

public record AccessToken(Guid UserId, Guid? CompanyAccessId, DateTimeOffset ExpiredAt);
