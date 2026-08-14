using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;

namespace ApiProvaOnline.Application.Services
{
    public class AlunoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAlunoRepository _repository;

        public AlunoService(IUnitOfWork unitOfWork, IAlunoRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task CadastrarAlunoAsync(Aluno aluno)
        {
            aluno.DataHoraRegistro = DateTime.Now;
            await _repository.AddAsync(aluno);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> RemoverAlunoAsync(int id)
        {
            var aluno = await _repository.GetByIdAsync(id);
            if (aluno == null) return false;

            _repository.Remove(aluno);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Aluno?> BuscarAlunoAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Aluno>> ListarAlunoAsync()
        {
            var alunos = await _repository.GetAllAsync();
            return alunos.OrderBy(a => a.Nome);
        }

        public async Task<bool> AtualizarAlunoAsync(Aluno aluno)
        {
            var existente = await _repository.GetByIdAsync(aluno.Id);
            if (existente == null) return false;

            existente.Nome = aluno.Nome;
            existente.DataNascimento = aluno.DataNascimento;
            existente.DataHoraRegistro = aluno.DataHoraRegistro;
            existente.IdProva = aluno.IdProva;

            _repository.Update(existente);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<HistoricoAlunoDTO>> HistoricoDeAlunoAsync(int alunoId)
        {
            return await _repository.GetHistoricoAsync(alunoId);
        }

        public async Task<IEnumerable<Aluno>> ListarPorProvaAsync(int provaId)
        {
            return await _repository.GetByProvaIdAsync(provaId);
        }

        public async Task<Aluno?> BuscarPorNomeDataNascimentoProvaAsync(string nome, DateTime dataNascimento, int provaId)
        {
            return await _repository.BuscarPorNomeDataNascimentoProvaAsync(nome, dataNascimento, provaId);
        }
    }
}



