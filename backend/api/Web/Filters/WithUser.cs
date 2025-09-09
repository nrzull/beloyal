using Beloyal.API.Auth;
using Beloyal.API.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithUserAttribute() : TypeFilterAttribute(typeof(WithUserFilter));

public class WithUserFilter(UserContext userContext) : IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
  {
    if (ctx.HttpContext.Items["accessToken"] is AccessToken token)
    {
      var user = await userContext.GetById(UserId.From(token.UserId));
      ctx.HttpContext.Items["user"] = user;
      ctx.ActionArguments["user"] = user;
    }

    await next();
  }
}
