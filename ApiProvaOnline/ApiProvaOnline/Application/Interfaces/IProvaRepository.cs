using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Interfaces
{
    public interface IProvaRepository : IRepository<Prova>
    {
        Task<IEnumerable<Prova>> GetByProfessorIdAsync(int professorId);
        Task<Prova?> GetByCodigoAsync(string codigo);
    }
}

