using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Interfaces
{
    public interface IAdministrativoRepository : IRepository<Administrativo>
    {
        Task<Administrativo?> GetByEmailAsync(string email);
    }
}

