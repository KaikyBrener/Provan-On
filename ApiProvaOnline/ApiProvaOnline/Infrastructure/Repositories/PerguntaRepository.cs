using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Infrastructure.Data;
using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProvaOnline.Infrastructure.Repositories
{
    public class PerguntaRepository : BaseRepository<Pergunta>, IPerguntaRepository
    {
        public PerguntaRepository(ProvaOnDbContext context) : base(context) { }

        public async Task<IEnumerable<Pergunta>> GetByProvaIdAsync(int provaId)
        {
            return await _context.Perguntas
                .Where(p => p.IdProva == provaId)
                .OrderBy(p => p.Titulo)
                .ToListAsync();
        }
    }
}


