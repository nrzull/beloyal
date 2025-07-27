namespace Beloyal.API.Companies;

public record CompanyAccessDTO(Guid Id, CompanyDTO Company, CompanyRoleDTO Role)
{
  public static CompanyAccessDTO From(CompanyAccess v) =>
    new(Id: v.Id, Company: CompanyDTO.From(v.Company), Role: CompanyRoleDTO.From(v.Role));
};
