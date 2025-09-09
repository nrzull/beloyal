using Beloyal.API.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithAdminAccessAttribute : TypeFilterAttribute
{
  public WithAdminAccessAttribute(string permission)
    : base(typeof(WithAdminAccessFilter))
  {
    Arguments = [permission];
  }
}

public class WithAdminAccessFilter(string permission) : IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
  {
    var user = (ctx.ActionArguments["user"] as User).Ensure();
    var phone = Environment.GetEnvironmentVariable("API_ADMIN_PHONE");

    if (
      user.Phone == phone
      || user.AdminAccess is { Role: { Permissions: var permissions } }
        && permissions.Any(v => v == permission)
    )
    {
      await next();
    }
    else
    {
      ctx.Result = new BadRequestObjectResult("no admin permissions");
    }
  }
}
