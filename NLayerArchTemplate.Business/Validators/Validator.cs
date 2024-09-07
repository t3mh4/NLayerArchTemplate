using FluentValidation;
using FluentValidation.Results;
using NLayerArchTemplate.Core.Extensions;

namespace NLayerArchTemplate.Business.Validators;

public static class Validator
{
    public static void Validate(ValidationResult result)
    {
        if (result.IsValid) return;
        throw new ValidationException(result.Errors.Select(s => new { s.PropertyName, s.ErrorMessage }).ToJSON());
    }
}
