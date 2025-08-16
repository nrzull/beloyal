namespace Beloyal.API.Companies;

public record CompanyAccessInvitationDTO(
  Guid Id,
  DateTimeOffset UpdatedAt,
  string Email,
  string Status,
  CompanyDTO Company,
  CompanyRoleDTO Role
)
{
  public static CompanyAccessInvitationDTO From(CompanyAccessInvitation v) =>
    new(
      Id: v.Id,
      UpdatedAt: v.UpdatedAt,
      Email: v.Email,
      Status: v.Status.ToString(),
      Company: CompanyDTO.From(v.Company),
      Role: CompanyRoleDTO.From(v.Role)
    );
};
