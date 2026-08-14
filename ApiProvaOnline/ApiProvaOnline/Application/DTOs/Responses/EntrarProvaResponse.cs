namespace ApiProvaOnline.Application.DTOs.Responses
{
    public class EntrarProvaResponse
    {
        public string Token { get; set; } = string.Empty;
        public AlunoDTO Aluno { get; set; } = default!;
        public ProvaDTO Prova { get; set; } = default!;
    }
}
