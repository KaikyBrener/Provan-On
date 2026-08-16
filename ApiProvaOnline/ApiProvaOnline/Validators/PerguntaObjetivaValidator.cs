using ApiProvaOnline.Models;
using FluentValidation;

namespace ApiProvaOnline.Validators
{
    public class PerguntaObjetivaValidator : AbstractValidator<PerguntaObjetiva>
    {
        private static readonly char[] OpcoesValidas = { 'A', 'B', 'C', 'D', 'a', 'b', 'c', 'd' };

        public PerguntaObjetivaValidator()
        {
            RuleFor(p => p.Titulo)
                .NotEmpty().WithMessage("O título da pergunta é obrigatório.")
                .MaximumLength(500).WithMessage("O título não pode ter mais de 500 caracteres.");

            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(500).WithMessage("A descrição não pode ter mais de 500 caracteres.");

            RuleFor(p => p.OpcaoA).NotEmpty().WithMessage("A opção A é obrigatória.");
            RuleFor(p => p.OpcaoB).NotEmpty().WithMessage("A opção B é obrigatória.");
            RuleFor(p => p.OpcaoC).NotEmpty().WithMessage("A opção C é obrigatória.");
            RuleFor(p => p.OpcaoD).NotEmpty().WithMessage("A opção D é obrigatória.");

            RuleFor(p => p.OpcaoCorreta)
                .Must(o => OpcoesValidas.Contains(o))
                .WithMessage("A opção correta deve ser A, B, C ou D.");

            RuleFor(p => p.Pontuacao)
                .GreaterThan(0).WithMessage("A pontuação deve ser maior que zero.");

            RuleFor(p => p.IdProva)
                .GreaterThan(0).WithMessage("A prova é obrigatória.");
        }
    }
}

