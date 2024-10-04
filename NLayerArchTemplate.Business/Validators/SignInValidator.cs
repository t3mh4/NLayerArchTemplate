using FluentValidation;
using NLayerArchTemplate.Dtos.Login;

namespace NLayerArchTemplate.Business.Validators;

public class SignInValidator : AbstractValidator<LoginDto>
{
    public SignInValidator()
    {
        RuleFor(x => x.Username).NotEmpty()
                                .CustomRule()
                                .WithName("Kullanıcı Adı");
        RuleFor(x => x.Password).NotEmpty()
                                .CustomRule()
                                .WithName("Şifre");
    }
}
