using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Interfaces
{
    public interface IAlunoRespostaObjetivaRepository : IRepository<AlunoRespostaObjetiva>
    {
        Task<IEnumerable<AlunoRespostaObjetiva>> GetByAlunoIdAsync(int alunoId);
        Task<IEnumerable<AlunoRespostaObjetiva>> GetByProvaIdAsync(int provaId);
        Task<IEnumerable<AlunoRespostaObjetiva>> GetByAlunoEProvaIdAsync(int alunoId, int provaId);
        Task<double> GetPontuacaoTotalAsync(int alunoId, int provaId);
        Task<bool> ExisteRespostaAsync(int alunoId, int perguntaObjetivaId);
    }
}

