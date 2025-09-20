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
    if (
      ctx.HttpContext.Items.ContainsKey("customerAccessToken")
      && ctx.HttpContext.Items["customerAccessToken"] is CustomerAccessToken token
    )
    {
      var customer = await customerContext.GetCustomer(CustomerId.From(token.CustomerId));

      ctx.HttpContext.Items["customer"] = customer;
      ctx.ActionArguments["customer"] = customer;
    }

    await next();
  }
}
