using FluentValidation;

namespace Beloyal.API.Types;

public class Percent
{
  public decimal Value { get; set; }
  bool WithZero { get; set; }

  public Percent(decimal value, bool withZero = true)
  {
    Value = Math.Round(value, 0);
    WithZero = withZero;

    try
    {
      new Validator().ValidateAndThrow(this);
    }
    catch
    {
      throw new Exception($"failed to validate Percent: ${Value}");
    }
  }

  class Validator : AbstractValidator<Percent>
  {
    public Validator()
    {
      RuleFor(v => v.Value)
        .GreaterThanOrEqualTo(0)
        .When(v => v.WithZero)
        .GreaterThan(0)
        .When(v => !v.WithZero)
        .LessThanOrEqualTo(100);
    }
  }
};
