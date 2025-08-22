namespace ApiProvaOnline.Model
{
    public class Prova
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public DateTime DataHoraInicio { get; set; }

        public DateTime DataHoraFim {  get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public string Codigo { get; set; }

        public int IdProfessor { get; set; }

    }
}
