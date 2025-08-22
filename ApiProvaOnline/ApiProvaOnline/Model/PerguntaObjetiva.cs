using System.Globalization;

namespace ApiProvaOnline.Model
{
    public class PerguntaObjetiva
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string OpcaoA {  get; set; }
        public string OpcaoB { get; set; }  
        public string OpcaoC { get; set; }
        public string OpcaoD { get; set; }
        public char OpcaoCorreta { get; set; }
        public double Pontuacao { get; set; }
        public DateTime DataHoraRegistro { get; set; }
        public int IdProva {  get; set; }

    }
}
