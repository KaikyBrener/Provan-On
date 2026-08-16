using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Infrastructure.Data;
using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProvaOnline.Infrastructure.Repositories
{
    public class ProvaRepository : BaseRepository<Prova>, IProvaRepository
    {
        public ProvaRepository(ProvaOnDbContext context) : base(context) { }

        public async Task<IEnumerable<Prova>> GetByProfessorIdAsync(int professorId)
        {
            return await _context.Provas
                .Where(p => p.IdProfessor == professorId)
                .OrderBy(p => p.Titulo)
                .ToListAsync();
        }

        public async Task<Prova?> GetByCodigoAsync(string codigo)
        {
            return await _context.Provas
                .Include(p => p.PerguntasObjetivas)
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }
    }
}


