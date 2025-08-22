namespace ApiProvaOnline.Model
{
    public class AlunoRespostaObjetiva
    {
        public int Id { get; set; }
        public int IdAluno { get; set; }
        public int IdPerguntaObjetiva { get; set; }
        public char Resposta {  get; set; }
        public double PontuacaoFinal { get; set; }
        public DateTime DataHorRegistro { get; set; }

        internal void AtualizarAlunoRespostaObjetiva(object administrativo)
        {
            throw new NotImplementedException();
        }
    }
}
