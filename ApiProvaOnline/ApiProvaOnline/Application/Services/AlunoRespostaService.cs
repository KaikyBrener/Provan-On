using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Services
{
    public class AlunoRespostaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAlunoRespostaRepository _repository;

        public AlunoRespostaService(IUnitOfWork unitOfWork, IAlunoRespostaRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task CadastrarAlunoRespostaAsync(AlunoResposta alunoResposta)
        {
            alunoResposta.DataHoraRegistro = DateTime.Now;
            await _repository.AddAsync(alunoResposta);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> RemoverAlunoRespostaAsync(int id)
        {
            var resposta = await _repository.GetByIdAsync(id);
            if (resposta == null) return false;

            _repository.Remove(resposta);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<AlunoResposta?> BuscarAlunoRespostaAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> AtualizarAlunoRespostaAsync(AlunoResposta alunoResposta)
        {
            var existente = await _repository.GetByIdAsync(alunoResposta.Id);
            if (existente == null) return false;

            existente.IdAluno = alunoResposta.IdAluno;
            existente.IdPergunta = alunoResposta.IdPergunta;
            existente.Resposta = alunoResposta.Resposta;
            existente.PontuacaoFinal = alunoResposta.PontuacaoFinal;
            existente.DataHoraRegistro = alunoResposta.DataHoraRegistro;

            _repository.Update(existente);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}


