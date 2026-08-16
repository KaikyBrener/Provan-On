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
            // Senha é armazenada com hash (BCrypt) — não é possível comparar em SQL.
            // Busca por e-mail e valida o hash em memória, mantendo o mesmo
            // resultado (Professor? autenticado ou null) que o fluxo atual
            // de ProfessorService.LoginProfessorAsync espera.
            var professor = await _context.Professores
                .FirstOrDefaultAsync(p => p.Email == email);

            if (professor != null && BCrypt.Net.BCrypt.Verify(senha, professor.Senha))
            {
                return professor;
            }

            return null;
        }
    }
}


