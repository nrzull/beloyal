using Beloyal.API.Companies;

namespace Beloyal.API.Web.CompanyAccesses.GetAccesses;

public abstract record GetAccessesResponse
{
  public record Ok(List<CompanyAccessDTO> Data) : GetAccessesResponse;
}
