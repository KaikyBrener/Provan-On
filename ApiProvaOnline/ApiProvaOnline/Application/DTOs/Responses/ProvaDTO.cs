namespace ApiProvaOnline.Application.DTOs.Responses
{
    public class ProvaDTO
    {
        public int Id { get; set; }
        public int IdProfessor { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public DateTime DataHoraRegistro { get; set; }
    }
}
