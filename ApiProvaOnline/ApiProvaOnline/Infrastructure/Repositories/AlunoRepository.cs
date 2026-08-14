using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Infrastructure.Data;
using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using ApiProvaOnline.Application.DTOs;

namespace ApiProvaOnline.Infrastructure.Repositories
{
    public class AlunoRepository : BaseRepository<Aluno>, IAlunoRepository
    {
        public AlunoRepository(ProvaOnDbContext context) : base(context) { }

        public async Task<IEnumerable<Aluno>> GetByProvaIdAsync(int provaId)
        {
            return await _context.Alunos
                .Where(a => a.IdProva == provaId)
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistoricoAlunoDTO>> GetHistoricoAsync(int alunoId)
        {
            return await _context.AlunoRespostasObjetivas
                .Where(ar => ar.IdAluno == alunoId)
                .Include(ar => ar.Aluno)
                .Include(ar => ar.PerguntaObjetiva)
                    .ThenInclude(po => po!.Prova)
                .OrderBy(ar => ar.PerguntaObjetiva!.IdProva)
                .ThenBy(ar => ar.IdPerguntaObjetiva)
                .Select(ar => new HistoricoAlunoDTO
                {
                    id = ar.IdAluno,
                    nome = ar.Aluno!.Nome,
                    prova = ar.PerguntaObjetiva!.Prova!.Titulo,
                    pergunta = ar.PerguntaObjetiva.Titulo,
                    opcaocorreta = ar.PerguntaObjetiva.OpcaoCorreta,
                    pontuacaofinal = ar.PontuacaoFinal
                })
                .ToListAsync();
        }

        public async Task<Aluno?> BuscarPorNomeDataNascimentoProvaAsync(string nome, DateTime dataNascimento, int provaId)
        {
            return await _context.Alunos
                .FirstOrDefaultAsync(a => 
                    a.Nome == nome && 
                    a.DataHoraRegistro.Date == dataNascimento.Date && 
                    a.IdProva == provaId);
        }
    }
}



