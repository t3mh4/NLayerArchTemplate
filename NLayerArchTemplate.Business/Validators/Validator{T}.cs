using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NLayerArchTemplate.Core.Extensions;

namespace NLayerArchTemplate.Business.Validators;

public static class Validator<T> where T : new()
{
    public static void Validate<E>(E entity, IServiceProvider serviceProvider) where E : new()
    {
        var validator = (IValidator<E>)ActivatorUtilities.CreateInstance(serviceProvider, typeof(T));
        var result = validator.Validate(entity);
        if (result.IsValid) return;
        throw new ValidationException(result.Errors.Select(s => new { s.PropertyName, s.ErrorMessage }).ToJSON());
    }
}
