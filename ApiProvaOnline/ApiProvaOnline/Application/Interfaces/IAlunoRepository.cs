using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;

namespace ApiProvaOnline.Application.Interfaces
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        Task<IEnumerable<Aluno>> GetByProvaIdAsync(int provaId);
        Task<IEnumerable<HistoricoAlunoDTO>> GetHistoricoAsync(int alunoId);
        Task<Aluno?> BuscarPorNomeDataNascimentoProvaAsync(string nome, DateTime dataNascimento, int provaId);
    }
}


