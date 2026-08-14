namespace ApiProvaOnline.Models
{
    public class PerguntaObjetiva : BaseEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string OpcaoA { get; set; } = string.Empty;
        public string OpcaoB { get; set; } = string.Empty;
        public string OpcaoC { get; set; } = string.Empty;
        public string OpcaoD { get; set; } = string.Empty;
        public char OpcaoCorreta { get; set; }
        public double Pontuacao { get; set; }
        public DateTime DataHoraRegistro { get; set; }
        public int IdProva { get; set; }

        // Navigation properties
        public Prova? Prova { get; set; }
        public ICollection<AlunoRespostaObjetiva> RespostasObjetivas { get; set; } = new List<AlunoRespostaObjetiva>();
    }
}


