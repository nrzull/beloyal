namespace Beloyal.API.Web.Operations.CancelOperation;

public abstract record CancelOperationResponse
{
  public record Ok : CancelOperationResponse;
}
