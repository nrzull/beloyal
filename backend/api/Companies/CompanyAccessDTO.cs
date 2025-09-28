using Beloyal.API.Users;

namespace Beloyal.API.Companies;

public record CompanyAccessDTO(
  Guid Id,
  CompanyDTO Company,
  CompanyRoleDTO Role,
  UserDTO User,
  DateTimeOffset? BlockedAt
)
{
  public static CompanyAccessDTO From(CompanyAccess v) =>
    new(
      Id: v.Id,
      Company: CompanyDTO.From(v.Company),
      Role: CompanyRoleDTO.From(v.Role),
      User: UserDTO.From(v.User),
      BlockedAt: v.BlockedAt
    );
};
