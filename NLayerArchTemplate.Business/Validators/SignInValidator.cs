using FluentValidation;
using NLayerArchTemplate.Dtos.Login;

namespace NLayerArchTemplate.Business.Validators;

public class SignInValidator : AbstractValidator<LoginDto>
{
    public SignInValidator()
    {
         RuleFor(x => x.Username).NotEmpty()
                         .Matches(@"[^'\x22]+")
                         .WithMessage("'{PropertyName}' için özel karakter kullanmayınız.")
                         .WithName("Kullanıcı Adı");
		RuleFor(x => x.Password).NotEmpty()
                         .Matches(@"[^'\x22]+")
                         .WithMessage("'{PropertyName}' için özel karakter kullanmayınız.")
                         .WithName("Şifre");
    }
}
