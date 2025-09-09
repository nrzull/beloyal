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
        .Must(v => v.Split().All(v => int.TryParse(v, out var _)))
        .Must(v => v.Count() == 10 || v.Count() == 12);
    }
  }
};
