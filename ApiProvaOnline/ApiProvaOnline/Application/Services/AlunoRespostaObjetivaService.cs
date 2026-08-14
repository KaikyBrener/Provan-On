using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Services
{
    public class AlunoRespostaObjetivaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAlunoRespostaObjetivaRepository _repository;
        private readonly IPerguntaObjetivaRepository _perguntaRepository;

        public AlunoRespostaObjetivaService(IUnitOfWork unitOfWork, 
            IAlunoRespostaObjetivaRepository repository,
            IPerguntaObjetivaRepository perguntaRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _perguntaRepository = perguntaRepository;
        }

        public async Task CadastrarAlunoRespostaObjetivaAsync(AlunoRespostaObjetiva alunoRespostaObjetiva)
        {
            alunoRespostaObjetiva.DataHorRegistro = DateTime.Now;

            var pergunta = await _perguntaRepository.GetByIdAsync(alunoRespostaObjetiva.IdPerguntaObjetiva);
            if (pergunta != null)
            {
                var acertou = char.ToUpper(alunoRespostaObjetiva.Resposta) == char.ToUpper(pergunta.OpcaoCorreta);
                alunoRespostaObjetiva.PontuacaoFinal = acertou ? pergunta.Pontuacao : 0;
            }

            await _repository.AddAsync(alunoRespostaObjetiva);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> RemoverAlunoRespostaObjetivaAsync(int id)
        {
            var resposta = await _repository.GetByIdAsync(id);
            if (resposta == null) return false;

            _repository.Remove(resposta);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<AlunoRespostaObjetiva?> BuscarAlunoRespostaObjetivaAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> AtualizarAlunoRespostaObjetivaAsync(AlunoRespostaObjetiva alunoRespostaObjetiva)
        {
            var existente = await _repository.GetByIdAsync(alunoRespostaObjetiva.Id);
            if (existente == null) return false;

            existente.IdAluno = alunoRespostaObjetiva.IdAluno;
            existente.IdPerguntaObjetiva = alunoRespostaObjetiva.IdPerguntaObjetiva;
            existente.Resposta = alunoRespostaObjetiva.Resposta;
            existente.PontuacaoFinal = alunoRespostaObjetiva.PontuacaoFinal;
            existente.DataHorRegistro = alunoRespostaObjetiva.DataHorRegistro;

            _repository.Update(existente);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<AlunoRespostaObjetiva>> ListarPorAlunoAsync(int alunoId)
        {
            return await _repository.GetByAlunoIdAsync(alunoId);
        }
    }
}


