using Beloyal.API.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithUserAttribute() : TypeFilterAttribute(typeof(WithUserFilter));

public class WithUserFilter(UserContext userContext) : IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
  {
    if (ctx.HttpContext.Items["userId"] is Guid id)
    {
      ctx.ActionArguments["user"] = await userContext.GetById(id);
    }

    await next();
  }
}
