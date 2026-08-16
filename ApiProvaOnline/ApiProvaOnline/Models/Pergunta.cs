namespace ApiProvaOnline.Models
{
    public class Pergunta : BaseEntity
    {

        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public double Pontuacao { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public int IdProva { get; set; }

        // Navigation properties
        public Prova? Prova { get; set; }
        public ICollection<AlunoResposta> Respostas { get; set; } = new List<AlunoResposta>();
    }
}


