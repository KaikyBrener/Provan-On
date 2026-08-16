using ApiProvaOnline.Models;
using FluentValidation;

namespace ApiProvaOnline.Validators
{
    public class AlunoRespostaObjetivaValidator : AbstractValidator<AlunoRespostaObjetiva>
    {
        private static readonly char[] OpcoesValidas = { 'A', 'B', 'C', 'D', 'a', 'b', 'c', 'd' };

        public AlunoRespostaObjetivaValidator()
        {
            RuleFor(r => r.IdAluno)
                .GreaterThan(0).WithMessage("O aluno é obrigatório.");

            RuleFor(r => r.IdPerguntaObjetiva)
                .GreaterThan(0).WithMessage("A pergunta é obrigatória.");

            RuleFor(r => r.Resposta)
                .Must(r => OpcoesValidas.Contains(r))
                .WithMessage("A resposta deve ser A, B, C ou D.");
        }
    }
}

