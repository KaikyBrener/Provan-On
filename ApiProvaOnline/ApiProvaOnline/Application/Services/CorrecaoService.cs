using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using ApiProvaOnline.Application.DTOs;

namespace ApiProvaOnline.Application.Services
{
    public class CorrecaoService : ICorrecaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAlunoRespostaObjetivaRepository _respostaRepository;
        private readonly IPerguntaObjetivaRepository _perguntaRepository;
        private readonly IAlunoRespostaRepository _alunoRespostaRepository;

        public CorrecaoService(IUnitOfWork unitOfWork, 
            IAlunoRespostaObjetivaRepository respostaRepository,
            IPerguntaObjetivaRepository perguntaRepository,
            IAlunoRespostaRepository alunoRespostaRepository)
        {
            _unitOfWork = unitOfWork;
            _respostaRepository = respostaRepository;
            _perguntaRepository = perguntaRepository;
            _alunoRespostaRepository = alunoRespostaRepository;
        }

        public async Task<ApiProvaOnline.Application.DTOs.Responses.ResultadoParcialDTO> CalcularPontuacaoAsync(int alunoId, int provaId)
        {
            var respostasObj = (await _respostaRepository.GetByAlunoEProvaIdAsync(alunoId, provaId)).ToList();

            var totalObjetivasProva = await _perguntaRepository.GetByProvaIdAsync(provaId); // Note: _perguntaRepository here is IPerguntaObjetivaRepository
            var notaParcial = respostasObj.Sum(r => r.PontuacaoFinal);
            var acertos = respostasObj.Count(r => r.PontuacaoFinal > 0);
            var erros = respostasObj.Count(r => r.PontuacaoFinal == 0);

            var respostasDiscursivas = (await _alunoRespostaRepository.GetByAlunoEProvaIdAsync(alunoId, provaId)).ToList();
            var pendentes = respostasDiscursivas.Count(r => r.PontuacaoFinal == null || r.PontuacaoFinal == 0); // Presume-se 0 ou null quando não corrigido, dependendo do modelo.

            return new ApiProvaOnline.Application.DTOs.Responses.ResultadoParcialDTO
            {
                NotaParcial = notaParcial,
                Acertos = acertos,
                Erros = erros,
                DiscursivasPendentes = pendentes
            };
        }

        public async Task CorrigirProvaAsync(int alunoId, int provaId)
        {
            var respostasDaProva = (await _respostaRepository.GetByAlunoEProvaIdAsync(alunoId, provaId)).ToList();

            foreach (var resposta in respostasDaProva)
            {
                if (resposta.PerguntaObjetiva == null) continue;

                var acertou = char.ToUpper(resposta.Resposta) == char.ToUpper(resposta.PerguntaObjetiva.OpcaoCorreta);
                resposta.PontuacaoFinal = acertou ? resposta.PerguntaObjetiva.Pontuacao : 0;
                _respostaRepository.Update(resposta);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<AlunoPontuacaoDTO>> ListarPontuacoesProvaAsync(int provaId)
        {
            var respostas = await _respostaRepository.GetByProvaIdAsync(provaId);

            return respostas
                .GroupBy(r => new { r.IdAluno, NomeAluno = r.Aluno?.Nome ?? string.Empty, Prova = r.PerguntaObjetiva?.Prova?.Titulo ?? string.Empty })
                .Select(g => new AlunoPontuacaoDTO
                {
                    aluno = g.Key.NomeAluno,
                    Titulo = g.Key.Prova,
                    PontuacaoFinal = g.Sum(r => r.PontuacaoFinal).ToString("F2")
                });
        }
    }
}



