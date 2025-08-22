namespace ApiProvaOnline.Model
{
    public class AlunoResposta
    {

        public int Id { get; set; }

        public int IdAluno { get; set; }

        public int IdPergunta { get; set; }

        public string Resposta { get; set; }

        public string PontuacaoFinal { get; set; }

        public DateTime DataHoraRegistro { get; set; }

    }
}
