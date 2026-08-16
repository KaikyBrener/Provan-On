using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Services
{
    public class PerguntaObjetivaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPerguntaObjetivaRepository _repository;

        public PerguntaObjetivaService(IUnitOfWork unitOfWork, IPerguntaObjetivaRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task CadastrarPerguntaObjetivaAsync(PerguntaObjetiva perguntaObjetiva)
        {
            perguntaObjetiva.DataHoraRegistro = DateTime.Now;
            await _repository.AddAsync(perguntaObjetiva);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> RemoverPerguntaObjetivaAsync(int id)
        {
            var pergunta = await _repository.GetByIdAsync(id);
            if (pergunta == null) return false;

            _repository.Remove(pergunta);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<PerguntaObjetiva?> BuscarPerguntaObjetivaAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<PerguntaObjetiva>> ListarPerguntasObjetivasAsync()
        {
            var perguntas = await _repository.GetAllAsync();
            return perguntas.OrderBy(p => p.Titulo);
        }

        public async Task<IEnumerable<PerguntaObjetiva>> ListarPorProvaAsync(int provaId)
        {
            return await _repository.GetByProvaIdAsync(provaId);
        }

        public async Task<bool> AtualizarPerguntaObjetivaAsync(PerguntaObjetiva perguntaObjetiva)
        {
            var existente = await _repository.GetByIdAsync(perguntaObjetiva.Id);
            if (existente == null) return false;

            existente.Titulo = perguntaObjetiva.Titulo;
            existente.Descricao = perguntaObjetiva.Descricao;
            existente.OpcaoA = perguntaObjetiva.OpcaoA;
            existente.OpcaoB = perguntaObjetiva.OpcaoB;
            existente.OpcaoC = perguntaObjetiva.OpcaoC;
            existente.OpcaoD = perguntaObjetiva.OpcaoD;
            existente.OpcaoCorreta = perguntaObjetiva.OpcaoCorreta;
            existente.Pontuacao = perguntaObjetiva.Pontuacao;
            existente.DataHoraRegistro = perguntaObjetiva.DataHoraRegistro;
            existente.IdProva = perguntaObjetiva.IdProva;

            _repository.Update(existente);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}


