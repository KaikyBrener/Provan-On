using ApiProvaOnline.Models;
using FluentValidation;

namespace ApiProvaOnline.Validators
{
    public class AlunoValidator : AbstractValidator<Aluno>
    {
        public AlunoValidator()
        {
            RuleFor(a => a.Nome)
                .NotEmpty().WithMessage("O nome do aluno é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

            RuleFor(a => a.DataNascimento)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
                .LessThan(DateTime.Today).WithMessage("A data de nascimento deve ser no passado.");

            RuleFor(a => a.IdProva)
                .GreaterThan(0).WithMessage("A prova é obrigatória.");
        }
    }
}

