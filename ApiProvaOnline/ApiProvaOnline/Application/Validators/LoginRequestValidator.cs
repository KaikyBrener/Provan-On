using ApiProvaOnline.Application.DTOs.Requests;
using FluentValidation;

namespace ApiProvaOnline.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail informado não é válido.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
        }
    }
}
