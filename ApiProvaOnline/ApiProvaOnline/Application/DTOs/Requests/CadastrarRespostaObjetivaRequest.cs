namespace ApiProvaOnline.Application.DTOs.Requests
{
    /// <summary>
    /// DTO para submissão de resposta objetiva pelo aluno.
    /// AlternativaSelecionada: 0=A, 1=B, 2=C, 3=D.
    /// </summary>
    public class CadastrarRespostaObjetivaRequest
    {
        public int AlunoId { get; set; }
        public int PerguntaObjetivaId { get; set; }
        public int AlternativaSelecionada { get; set; }
        public int ProvaId { get; set; }
    }
}
