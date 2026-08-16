using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Services
{
    public class ProfessorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfessorRepository _repository;

        public ProfessorService(IUnitOfWork unitOfWork, IProfessorRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task CadastrarProfessorAsync(Professor professor)
        {
            professor.Senha = BCrypt.Net.BCrypt.HashPassword(professor.Senha);
            professor.DataHoraRegistro = DateTime.Now;
            await _repository.AddAsync(professor);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> RemoverProfessorAsync(int id)
        {
            var professor = await _repository.GetByIdAsync(id);
            if (professor == null) return false;

            _repository.Remove(professor);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Professor?> BuscarProfessorAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Professor>> ListarProfessoresAsync()
        {
            var professores = await _repository.GetAllAsync();
            return professores.OrderBy(p => p.Nome);
        }

        public async Task<bool> AtualizarProfessorAsync(Professor professor)
        {
            var existente = await _repository.GetByIdAsync(professor.Id);
            if (existente == null) return false;

            existente.Nome = professor.Nome;
            existente.Email = professor.Email;
            
            if (!string.IsNullOrEmpty(professor.Senha) && !professor.Senha.StartsWith("$2a$"))
            {
                existente.Senha = BCrypt.Net.BCrypt.HashPassword(professor.Senha);
            }

            existente.DataHoraRegistro = professor.DataHoraRegistro;

            _repository.Update(existente);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Professor?> LoginProfessorAsync(string email, string senha)
        {
            // _repository.LoginAsync probably checks BOTH email and password in DB.
            // We need to fetch by email first, then verify.
            var professores = await _repository.FindAsync(p => p.Email == email);
            var professor = professores.FirstOrDefault();
            
            if (professor != null && BCrypt.Net.BCrypt.Verify(senha, professor.Senha))
            {
                return professor;
            }
            return null;
        }
    }
}


