using Beloyal.API.Companies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithCompanyPermissionsAttribute : TypeFilterAttribute
{
  public WithCompanyPermissionsAttribute(string permission)
    : base(typeof(WithCompanyPermissionsFilter))
  {
    Arguments = [permission];
  }
}

public class WithCompanyPermissionsFilter(string permission) : IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
  {
    var companyAccess = (CompanyAccess)ctx.ActionArguments["companyAccess"]!;

    if (
      companyAccess.UserId == companyAccess.Company.OwnerId
      || companyAccess.Role.Permissions.Any(v => v == permission)
    )
    {
      await next();
    }
    else
    {
      ctx.Result = new BadRequestObjectResult("no company permissions");
    }
  }
}
