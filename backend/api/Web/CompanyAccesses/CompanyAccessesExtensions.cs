using Beloyal.API.Web.CompanyAccesses.BlockAccess;
using Beloyal.API.Web.CompanyAccesses.CreateAccess;
using Beloyal.API.Web.CompanyAccesses.GetAccess;
using Beloyal.API.Web.CompanyAccesses.GetAccesses;
using Beloyal.API.Web.CompanyAccesses.RemoveAccess;
using Beloyal.API.Web.CompanyAccesses.UnblockAccess;
using Beloyal.API.Web.CompanyAccesses.UpdateAccess;

namespace Beloyal.API.Web.CompanyAccesses;

public static class CompanyAccessesExtensions
{
  public static void UseCompanyAccessesEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateAccessEndpoint>();
    builder.Services.AddScoped<UpdateAccessEndpoint>();
    builder.Services.AddScoped<RemoveAccessEndpoint>();
    builder.Services.AddScoped<GetAccessesEndpoint>();
    builder.Services.AddScoped<GetAccessEndpoint>();
    builder.Services.AddScoped<BlockAccessEndpoint>();
    builder.Services.AddScoped<UnblockAccessEndpoint>();
  }
}
