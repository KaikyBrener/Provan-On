using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Services
{
    public class AdministrativoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdministrativoRepository _repository;

        public AdministrativoService(IUnitOfWork unitOfWork, IAdministrativoRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task CadastrarAdministrativoAsync(Administrativo administrativo)
        {
            administrativo.Senha = BCrypt.Net.BCrypt.HashPassword(administrativo.Senha);
            administrativo.DataHoraRegistro = DateTime.Now;
            await _repository.AddAsync(administrativo);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> RemoverAdministrativoAsync(int id)
        {
            var administrativo = await _repository.GetByIdAsync(id);
            if (administrativo == null) return false;

            _repository.Remove(administrativo);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Administrativo?> BuscarAdministrativoAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Administrativo>> ListarAdministrativoAsync()
        {
            var administrativos = await _repository.GetAllAsync();
            return administrativos.OrderBy(a => a.Nome);
        }

        public async Task<bool> AtualizarAdministrativoAsync(Administrativo administrativo)
        {
            var existente = await _repository.GetByIdAsync(administrativo.Id);
            if (existente == null) return false;

            existente.Nome = administrativo.Nome;
            existente.Email = administrativo.Email;

            if (!string.IsNullOrEmpty(administrativo.Senha) && !administrativo.Senha.StartsWith("$2a$"))
            {
                existente.Senha = BCrypt.Net.BCrypt.HashPassword(administrativo.Senha);
            }

            existente.DataHoraRegistro = administrativo.DataHoraRegistro;

            _repository.Update(existente);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Administrativo?> LoginAdministrativoAsync(string email, string senha)
        {
            var admins = await _repository.FindAsync(a => a.Email == email);
            var admin = admins.FirstOrDefault();
            
            if (admin != null && BCrypt.Net.BCrypt.Verify(senha, admin.Senha))
            {
                return admin;
            }
            return null;
        }
    }
}


