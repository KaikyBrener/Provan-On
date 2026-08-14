namespace ApiProvaOnline.Application.DTOs.Requests
{
    public class EntrarProvaRequest
    {
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string CodigoProva { get; set; } = string.Empty;
    }
}
