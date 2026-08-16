namespace ApiProvaOnline.Models
{
    public class AlunoResposta : BaseEntity
    {

        public int IdAluno { get; set; }

        public int IdPergunta { get; set; }

        public string Resposta { get; set; } = string.Empty;

        public double PontuacaoFinal { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        // Navigation properties
        public Aluno? Aluno { get; set; }
        public Pergunta? Pergunta { get; set; }
    }
}


