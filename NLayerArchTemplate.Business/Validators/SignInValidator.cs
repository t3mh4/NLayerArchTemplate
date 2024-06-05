using FluentValidation;
using NLayerArchTemplate.Dtos.Login;

namespace NtierArchTemplate.Business.Validators;

public class SignInValidator : AbstractValidator<LoginDto>
{
    public SignInValidator()
    {
        RuleFor(x => x.Username).NotEmpty().NotNull().WithName("Kullanıcı Adı");
        RuleFor(x => x.Password).NotEmpty().NotNull().WithName("Şifre");
    }
}
