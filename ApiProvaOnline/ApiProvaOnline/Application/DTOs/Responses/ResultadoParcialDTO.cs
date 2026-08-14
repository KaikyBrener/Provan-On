namespace ApiProvaOnline.Application.DTOs.Responses
{
    public class ResultadoParcialDTO
    {
        public double NotaParcial { get; set; }
        public int Acertos { get; set; }
        public int Erros { get; set; }
        public int DiscursivasPendentes { get; set; }
    }
}
