namespace ApiProvaOnline.Model
{
    public class Pergunta
    {

        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public double Pontuacao { get; set; }

        public DateTime DataHoraRegistro {  get; set; }
        
        public int IdProva {  get; set; }

    }
}
