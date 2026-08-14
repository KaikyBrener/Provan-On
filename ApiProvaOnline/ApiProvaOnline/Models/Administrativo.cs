namespace ApiProvaOnline.Models
{
    public class Administrativo : BaseEntity
    {

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public DateTime DataHoraRegistro { get; set; }
    }
}


