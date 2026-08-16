using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Interfaces
{
    public interface IAlunoRespostaRepository : IRepository<AlunoResposta>
    {
        Task<IEnumerable<AlunoResposta>> GetByAlunoIdAsync(int alunoId);
        Task<IEnumerable<AlunoResposta>> GetByAlunoEProvaIdAsync(int alunoId, int provaId);
        Task<bool> ExisteRespostaAsync(int alunoId, int perguntaId);
    }
}

