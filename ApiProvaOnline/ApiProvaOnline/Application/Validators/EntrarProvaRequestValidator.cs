using ApiProvaOnline.Application.DTOs.Requests;
using FluentValidation;

namespace ApiProvaOnline.Application.Validators
{
    public class EntrarProvaRequestValidator : AbstractValidator<EntrarProvaRequest>
    {
        public EntrarProvaRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(2).WithMessage("O nome deve ter no mínimo 2 caracteres.")
                .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

            RuleFor(x => x.DataNascimento)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
                .LessThan(DateTime.Today).WithMessage("A data de nascimento deve ser anterior à data atual.");

            RuleFor(x => x.CodigoProva)
                .NotEmpty().WithMessage("O código da prova é obrigatório.")
                .MaximumLength(45).WithMessage("O código da prova deve ter no máximo 45 caracteres.");
        }
    }
}
