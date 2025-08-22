namespace ApiProvaOnline.Model
{
    public class HistoricoAlunoDTO
    {

        public int id { get; set; }
        public string nome { get; set; }
        public string prova { get; set; }
        public string pergunta { get; set; }
        public char opcaocorreta { get; set; }
        public double pontuacaofinal { get; set; }

    }
}
