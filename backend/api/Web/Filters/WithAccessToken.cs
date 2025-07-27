using Beloyal.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithAccessTokenAttribute() : TypeFilterAttribute(typeof(WithAccessTokenFilter));

public class WithAccessTokenFilter() : IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
  {
    if (ctx.HttpContext.Items["accessToken"] is AccessToken token)
    {
      ctx.ActionArguments["accessToken"] = token;
    }

    await next();
  }
}
