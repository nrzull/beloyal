using Beloyal.API.Auth;
using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithShopAccessAttribute() : TypeFilterAttribute(typeof(WithShopAccessFilter));

public class WithShopAccessFilter(ShopContext shopContext) : IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
  {
    if (ctx.HttpContext.Items["companyAccess"] is not CompanyAccess companyAccess)
    {
      ctx.Result = new BadRequestObjectResult("invalid companyAccess");
      return;
    }

    if (ctx.HttpContext.Items["accessToken"] is not AccessToken token)
    {
      ctx.Result = new BadRequestObjectResult("invalid accessToken");
      return;
    }

    if (token.ShopAccessId is not Guid shopAccessId)
    {
      ctx.Result = new BadRequestObjectResult("invalid shopAccessId");
      return;
    }

    var shopAccess = await shopContext.GetAccess(ShopAccessId.From(shopAccessId));
    if (shopAccess is null || shopAccess.CompanyAccessId != companyAccess.Id)
    {
      ctx.Result = new BadRequestObjectResult("invalid shop access");
      return;
    }

    var shop = await shopContext.GetShop(ShopAccessId.From(shopAccessId));
    if (shop is null || shop.CompanyId != companyAccess.CompanyId)
    {
      ctx.Result = new BadRequestObjectResult("invalid shop");
      return;
    }

    ctx.HttpContext.Items["shopAccess"] = shopAccess;
    ctx.HttpContext.Items["shop"] = shop;

    ctx.ActionArguments["shopAccess"] = shopAccess;
    ctx.ActionArguments["shop"] = shop;

    await next();
  }
}
