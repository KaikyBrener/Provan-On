using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Models;

namespace ApiProvaOnline.Application.Services
{
    public class PerguntaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPerguntaRepository _repository;

        public PerguntaService(IUnitOfWork unitOfWork, IPerguntaRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task CadastrarPerguntaAsync(Pergunta pergunta)
        {
            pergunta.DataHoraRegistro = DateTime.Now;
            await _repository.AddAsync(pergunta);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> RemoverPerguntaAsync(int id)
        {
            var pergunta = await _repository.GetByIdAsync(id);
            if (pergunta == null) return false;

            _repository.Remove(pergunta);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Pergunta?> BuscarPerguntaAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Pergunta>> ListarPerguntaAsync()
        {
            var perguntas = await _repository.GetAllAsync();
            return perguntas.OrderBy(p => p.DataHoraRegistro);
        }

        public async Task<IEnumerable<Pergunta>> ListarPorProvaAsync(int provaId)
        {
            return await _repository.GetByProvaIdAsync(provaId);
        }

        public async Task<bool> AtualizarPerguntaAsync(Pergunta pergunta)
        {
            var existente = await _repository.GetByIdAsync(pergunta.Id);
            if (existente == null) return false;

            existente.Titulo = pergunta.Titulo;
            existente.Descricao = pergunta.Descricao;
            existente.Pontuacao = pergunta.Pontuacao;
            existente.DataHoraRegistro = pergunta.DataHoraRegistro;
            existente.IdProva = pergunta.IdProva;

            _repository.Update(existente);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}


