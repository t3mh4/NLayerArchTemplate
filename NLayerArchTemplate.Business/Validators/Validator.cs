using FluentValidation;
using NLayerArchTemplate.Core.Extensions;

namespace NtierArchTemplate.Business.Validators;

public static class Validator<T> where T : new()
{
    public static void Validate<E>(E entity) where E : new()
    {
        var validator = (IValidator<E>)Activator.CreateInstance(typeof(T));
        var result = validator.Validate(entity);
        if (result.IsValid) return;
        throw new ValidationException(result.Errors.Select(s => new { s.PropertyName, s.ErrorMessage }).ToJSON());
    }
}
