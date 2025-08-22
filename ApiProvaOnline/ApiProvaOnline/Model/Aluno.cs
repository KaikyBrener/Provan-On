namespace ApiProvaOnline.Model
{
    public class Aluno
    {

        public int Id { get; set; }

        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public int IdProva { get; set; }

    }
}
