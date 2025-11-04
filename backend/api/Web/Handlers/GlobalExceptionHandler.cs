using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken
  )
  {
    logger.LogError(exception, "Exception occured: {Message}", exception.Message);

    var problemDetails = new ProblemDetails
    {
      Title = "Server error",
      Status = StatusCodes.Status500InternalServerError,
      Extensions = new Dictionary<string, object?>()
      {
        ["RequestId"] = httpContext.TraceIdentifier,
        ["TraceId"] = System.Diagnostics.Activity.Current?.Id,
      },
    };

    httpContext.Response.StatusCode = problemDetails.Status.Value;

    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }
}
