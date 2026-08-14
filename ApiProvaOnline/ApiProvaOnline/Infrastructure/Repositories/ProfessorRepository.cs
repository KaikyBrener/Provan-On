using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Infrastructure.Data;
using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProvaOnline.Infrastructure.Repositories
{
    public class ProfessorRepository : BaseRepository<Professor>, IProfessorRepository
    {
        public ProfessorRepository(ProvaOnDbContext context) : base(context) { }

        public async Task<Professor?> GetByEmailAsync(string email)
        {
            return await _context.Professores
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Professor?> LoginAsync(string email, string senha)
        {
            return await _context.Professores
                .FirstOrDefaultAsync(p => p.Email == email && p.Senha == senha);
        }
    }
}


