namespace Beloyal.API.Web.Self.UpdateSelf;

public record UpdateSelfRequest(
  string? FirstName,
  string? LastName,
  string? MiddleName,
  string CountryLang
);
