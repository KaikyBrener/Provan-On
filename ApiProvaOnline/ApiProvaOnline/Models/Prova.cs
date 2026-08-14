namespace ApiProvaOnline.Models
{
    public class Prova : BaseEntity
    {
        internal int? TotalQuestoes;

        public string Titulo { get; set; } = string.Empty;

        public DateTime DataHoraInicio { get; set; }

        public DateTime DataHoraFim { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public int IdProfessor { get; set; }

        // Navigation properties
        public Professor? Professor { get; set; }
        public ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();
        public ICollection<PerguntaObjetiva> PerguntasObjetivas { get; set; } = new List<PerguntaObjetiva>();
        public ICollection<Pergunta> Perguntas { get; set; } = new List<Pergunta>();
    }
}


