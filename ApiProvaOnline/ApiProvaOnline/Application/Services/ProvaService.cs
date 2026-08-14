using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using ApiProvaOnline.Application.DTOs;

namespace ApiProvaOnline.Application.Services
{
    public class ProvaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProvaRepository _repository;

        public ProvaService(IUnitOfWork unitOfWork, IProvaRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task CadastrarProvaAsync(Prova prova)
        {
            prova.DataHoraRegistro = DateTime.Now;
            await _repository.AddAsync(prova);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> RemoverProvaAsync(int id)
        {
            var prova = await _repository.GetByIdAsync(id);
            if (prova == null) return false;

            _repository.Remove(prova);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Prova?> BuscarProvaAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Prova>> ListarProvaAsync()
        {
            var provas = await _repository.GetAllAsync();
            return provas.OrderBy(p => p.Titulo);
        }

        public async Task<bool> AtualizarProvaAsync(Prova prova)
        {
            var existente = await _repository.GetByIdAsync(prova.Id);
            if (existente == null) return false;

            existente.Titulo = prova.Titulo;
            existente.DataHoraInicio = prova.DataHoraInicio;
            existente.DataHoraFim = prova.DataHoraFim;
            existente.DataHoraRegistro = prova.DataHoraRegistro;
            existente.Codigo = prova.Codigo;
            existente.IdProfessor = prova.IdProfessor;

            _repository.Update(existente);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<ProvasProfessorDTO>> ListarProvasProfessorAsync(int professorId)
        {
            var provas = await _repository.GetByProfessorIdAsync(professorId);
            return provas.Select(p => new ProvasProfessorDTO
            {
                Id = p.Id,
                Titulo = p.Titulo,
                DataHoraRegistro = p.DataHoraRegistro,
                Codigo = p.Codigo
            });
        }

        public async Task<Prova?> BuscarProvaPorCodigoAsync(string codigo)
        {
            return await _repository.GetByCodigoAsync(codigo);
        }
    }
}



