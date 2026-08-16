using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Infrastructure.Data;
using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProvaOnline.Infrastructure.Repositories
{
    public class AdministrativoRepository : BaseRepository<Administrativo>, IAdministrativoRepository
    {
        public AdministrativoRepository(ProvaOnDbContext context) : base(context) { }

        public async Task<Administrativo?> GetByEmailAsync(string email)
        {
            return await _context.Administrativos
                .FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}


