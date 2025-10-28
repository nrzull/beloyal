using Beloyal.API.Companies;
using Beloyal.API.Levels;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Levels.CreateLevel;
using Beloyal.API.Web.Levels.DeleteLevel;
using Beloyal.API.Web.Levels.GetLevel;
using Beloyal.API.Web.Levels.GetLevels;
using Beloyal.API.Web.Levels.UpdateLevel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Levels;

[ApiController]
[Route("/api/company/levels")]
[WithAuth]
[WithCompanyAccess]
public class LevelsController : ControllerBase
{
  [HttpGet]
  public async Task<Results<Ok<GetLevelsResponse.Ok>, BadRequest>> GetLevels(
    [FromServices] GetLevelsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetLevelsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpGet("{id}")]
  public async Task<Results<Ok<GetLevelResponse.Ok>, NotFound, BadRequest>> GetLevels(
    [FromServices] GetLevelEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid levelId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), LevelId.From(levelId)) switch
    {
      GetLevelResponse.Ok ok => TypedResults.Ok(ok),
      GetLevelResponse.NotFound => TypedResults.NotFound(),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost]
  public async Task<Results<Ok<CreateLevelResponse.Ok>, BadRequest>> CreateLevel(
    [FromServices] CreateLevelEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateLevelRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateLevelResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}")]
  public async Task<Results<Ok<UpdateLevelResponse.Ok>, BadRequest>> UpdateLevel(
    [FromServices] UpdateLevelEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid levelId,
    [FromBody] LevelDTO req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), LevelId.From(levelId), req) switch
    {
      UpdateLevelResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpDelete("{id}")]
  public async Task<Results<Ok<DeleteLevelResponse.Ok>, BadRequest>> DeleteLevel(
    [FromServices] DeleteLevelEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid levelId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), LevelId.From(levelId)) switch
    {
      DeleteLevelResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
