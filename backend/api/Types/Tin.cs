using FluentValidation;

namespace Beloyal.API.Types;

public class Tin
{
  public string Value { get; set; }

  public Tin(string value)
  {
    Value = value;

    try
    {
      new Validator().ValidateAndThrow(this);
    }
    catch (Exception)
    {
      Console.WriteLine($"failed to validate: {Value}");
      throw;
    }
  }

  class Validator : AbstractValidator<Tin>
  {
    public Validator()
    {
      RuleFor(v => v.Value)
        .NotEmpty()
        .Must(v => v.ToList().All(vv => int.TryParse(vv.ToString(), out var _)))
        .Must(v => v.Length == 10 || v.Length == 12);
    }
  }
};
