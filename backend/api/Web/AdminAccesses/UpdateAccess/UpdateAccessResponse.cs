using Beloyal.API.Admin;

namespace Beloyal.API.Web.AdminAccesses.UpdateAccess;

public abstract record UpdateAccessResponse
{
  public record Ok(AdminAccessDTO Data) : UpdateAccessResponse;
}
