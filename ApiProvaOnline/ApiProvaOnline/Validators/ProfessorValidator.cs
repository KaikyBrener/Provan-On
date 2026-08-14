using ApiProvaOnline.Models;
using FluentValidation;

namespace ApiProvaOnline.Validators
{
    public class ProfessorValidator : AbstractValidator<Professor>
    {
        public ProfessorValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome do professor é obrigatório.")
                .MaximumLength(150).WithMessage("O nome não pode ter mais de 150 caracteres.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail informado não é válido.")
                .MaximumLength(150).WithMessage("O e-mail não pode ter mais de 150 caracteres.");

            RuleFor(p => p.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}

