namespace Beloyal.API.Companies;

public record CompanyAccessInvitationDTO(
  Guid Id,
  DateTimeOffset UpdatedAt,
  string Email,
  CompanyAccessInvitationStatus Status,
  CompanyDTO Company,
  CompanyRoleDTO Role
)
{
  public static CompanyAccessInvitationDTO From(CompanyAccessInvitation v) =>
    new(
      Id: v.Id,
      UpdatedAt: v.UpdatedAt,
      Email: v.Email,
      Status: v.Status,
      Company: CompanyDTO.From(v.Company),
      Role: CompanyRoleDTO.From(v.Role)
    );
};
