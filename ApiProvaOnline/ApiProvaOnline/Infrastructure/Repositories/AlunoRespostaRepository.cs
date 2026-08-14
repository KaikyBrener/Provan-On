using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Infrastructure.Data;
using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProvaOnline.Infrastructure.Repositories
{
    public class AlunoRespostaRepository : BaseRepository<AlunoResposta>, IAlunoRespostaRepository
    {
        public AlunoRespostaRepository(ProvaOnDbContext context) : base(context) { }

        public async Task<IEnumerable<AlunoResposta>> GetByAlunoIdAsync(int alunoId)
        {
            return await _context.AlunoRespostas
                .Where(ar => ar.IdAluno == alunoId)
                .Include(ar => ar.Pergunta)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlunoResposta>> GetByAlunoEProvaIdAsync(int alunoId, int provaId)
        {
            return await _context.AlunoRespostas
                .Include(ar => ar.Pergunta)
                .Where(ar => ar.IdAluno == alunoId && ar.Pergunta!.IdProva == provaId)
                .ToListAsync();
        }
    }
}


