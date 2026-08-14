namespace ApiProvaOnline.Models
{
    public class Aluno : BaseEntity
    {

        public string Nome { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public int IdProva { get; set; }

        // Navigation properties
        public Prova? Prova { get; set; }
        public ICollection<AlunoRespostaObjetiva> RespostasObjetivas { get; set; } = new List<AlunoRespostaObjetiva>();
        public ICollection<AlunoResposta> Respostas { get; set; } = new List<AlunoResposta>();
    }
}


