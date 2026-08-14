using ApiProvaOnline.Models;
using FluentValidation;

namespace ApiProvaOnline.Validators
{
    public class ProvaValidator : AbstractValidator<Prova>
    {
        public ProvaValidator()
        {
            RuleFor(p => p.Titulo)
                .NotEmpty().WithMessage("O título da prova é obrigatório.")
                .MaximumLength(100).WithMessage("O título não pode ter mais de 100 caracteres.");

            RuleFor(p => p.Codigo)
                .NotEmpty().WithMessage("O código da prova é obrigatório.")
                .MaximumLength(45).WithMessage("O código não pode ter mais de 45 caracteres.");

            RuleFor(p => p.DataHoraInicio)
                .NotEmpty().WithMessage("A data/hora de início é obrigatória.");

            RuleFor(p => p.DataHoraFim)
                .NotEmpty().WithMessage("A data/hora de fim é obrigatória.")
                .GreaterThan(p => p.DataHoraInicio).WithMessage("A data/hora de fim deve ser posterior à de início.");

            RuleFor(p => p.IdProfessor)
                .GreaterThan(0).WithMessage("O professor é obrigatório.");
        }
    }
}

