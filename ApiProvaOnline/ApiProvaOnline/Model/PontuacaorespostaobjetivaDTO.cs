namespace ApiProvaOnline.Model
{
    public class PontuacaorespostaobjetivaDTO
    {

        public int idprova { get; set; }
        public string titulo { get; set; }
        public int idaluno { get; set; }
        public string aluno { get; set; }
        public int idpergunta { get; set; }
        public string titulopergunta { get; set; }
        public string descricaopergunta { get; set; }
        public char opcaocorreta { get; set; }
        public double pontuacao { get; set; }
        public char resposta { get; set; }
        public double pontuacaofinal { get; set; }

    }
}
