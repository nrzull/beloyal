using Beloyal.API.Auth;
using Beloyal.API.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithCustomerAttribute() : TypeFilterAttribute(typeof(WithCustomerFilter));

public class WithCustomerFilter(CustomerContext customerContext) : IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
  {
    if (ctx.HttpContext.Items["customerAccessToken"] is CustomerAccessToken token)
    {
      ctx.ActionArguments["customer"] = await customerContext.GetCustomer(
        CustomerId.From(token.CustomerId)
      );
    }

    await next();
  }
}
