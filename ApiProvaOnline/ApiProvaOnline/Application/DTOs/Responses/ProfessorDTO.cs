namespace ApiProvaOnline.Application.DTOs.Responses
{
    public class ProfessorDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataHoraRegistro { get; set; }
    }
}
