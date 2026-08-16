namespace ApiProvaOnline.Application.DTOs
{
    public class ListarPerguntarProvaDTO
    {
        public int id { get; set; }

        public string titulo { get; set; } = string.Empty;

        public string descricao { get; set; } = string.Empty;

        /// <summary>Alias de descricao — campo esperado pelo frontend (prova.js).</summary>
        public string enunciado { get; set; } = string.Empty;

        public string opcaoa { get; set; } = string.Empty;

        public string opcaob { get; set; } = string.Empty;

        public string opcaoc { get; set; } = string.Empty;

        public string opcaod { get; set; } = string.Empty;

        public double pontuacao { get; set; }

        /// <summary>Array de alternativas no formato esperado pelo frontend (prova.js).</summary>
        public List<string> alternativas { get; set; } = new();
    }
}


