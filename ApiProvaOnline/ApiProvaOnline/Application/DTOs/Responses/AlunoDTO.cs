namespace ApiProvaOnline.Application.DTOs.Responses
{
    public class AlunoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataHoraRegistro { get; set; }
        public int IdProva { get; set; }
    }
}
