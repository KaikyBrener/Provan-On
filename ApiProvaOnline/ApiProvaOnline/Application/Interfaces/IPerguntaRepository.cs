using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Interfaces
{
    public interface IPerguntaRepository : IRepository<Pergunta>
    {
        Task<IEnumerable<Pergunta>> GetByProvaIdAsync(int provaId);
    }
}

