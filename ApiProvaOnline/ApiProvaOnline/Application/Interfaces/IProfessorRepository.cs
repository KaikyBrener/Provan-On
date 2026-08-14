using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Interfaces
{
    public interface IProfessorRepository : IRepository<Professor>
    {
        Task<Professor?> GetByEmailAsync(string email);
        Task<Professor?> LoginAsync(string email, string senha);
    }
}

