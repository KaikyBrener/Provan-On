using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Infrastructure.Data;
using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProvaOnline.Infrastructure.Repositories
{
    public class AlunoRespostaObjetivaRepository : BaseRepository<AlunoRespostaObjetiva>, IAlunoRespostaObjetivaRepository
    {
        public AlunoRespostaObjetivaRepository(ProvaOnDbContext context) : base(context) { }

        public async Task<IEnumerable<AlunoRespostaObjetiva>> GetByAlunoIdAsync(int alunoId)
        {
            return await _context.AlunoRespostasObjetivas
                .Where(ar => ar.IdAluno == alunoId)
                .Include(ar => ar.PerguntaObjetiva)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlunoRespostaObjetiva>> GetByProvaIdAsync(int provaId)
        {
            return await _context.AlunoRespostasObjetivas
                .Include(ar => ar.PerguntaObjetiva)
                .Where(ar => ar.PerguntaObjetiva!.IdProva == provaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlunoRespostaObjetiva>> GetByAlunoEProvaIdAsync(int alunoId, int provaId)
        {
            return await _context.AlunoRespostasObjetivas
                .Include(ar => ar.PerguntaObjetiva)
                .Where(ar => ar.IdAluno == alunoId && ar.PerguntaObjetiva!.IdProva == provaId)
                .ToListAsync();
        }

        public async Task<double> GetPontuacaoTotalAsync(int alunoId, int provaId)
        {
            return await _context.AlunoRespostasObjetivas
                .Include(ar => ar.PerguntaObjetiva)
                .Where(ar => ar.IdAluno == alunoId && ar.PerguntaObjetiva!.IdProva == provaId)
                .SumAsync(ar => ar.PontuacaoFinal);
        }
    }
}


