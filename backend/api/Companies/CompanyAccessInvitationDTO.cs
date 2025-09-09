namespace Beloyal.API.Companies;

public record CompanyAccessInvitationDTO(
  Guid Id,
  DateTimeOffset UpdatedAt,
  string Phone,
  string Status,
  CompanyDTO Company,
  CompanyRoleDTO Role
)
{
  public static CompanyAccessInvitationDTO From(CompanyAccessInvitation v) =>
    new(
      Id: v.Id,
      UpdatedAt: v.UpdatedAt,
      Phone: v.Phone,
      Status: v.Status.ToString(),
      Company: CompanyDTO.From(v.Company),
      Role: CompanyRoleDTO.From(v.Role)
    );
};
