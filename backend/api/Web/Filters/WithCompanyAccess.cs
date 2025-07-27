using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithCompanyAccessAttribute() : TypeFilterAttribute(typeof(WithCompanyAccessFilter));

public class WithCompanyAccessFilter(CompanyContext companyContext) : IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
  {
    if (
      ctx.HttpContext.Items["accessToken"] is AccessToken token
      && token.CompanyAccessId is Guid id
    )
    {
      ctx.ActionArguments["companyAccess"] = await companyContext.GetAccess(id);
    }

    await next();
  }
}
