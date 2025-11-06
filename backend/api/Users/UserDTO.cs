namespace Beloyal.API.Users;

public record UserDTO(
  Guid Id,
  string? Phone,
  string? FirstName,
  string? LastName,
  string? MiddleName,
  string CountryLang
)
{
  public static UserDTO From(User v) =>
    new(
      Id: v.Id,
      Phone: v.Phone,
      FirstName: v.FirstName,
      LastName: v.LastName,
      MiddleName: v.MiddleName,
      CountryLang: v.CountryLang.ToString()
    );
};
