using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;

namespace ApiProvaOnline.Application.Interfaces
{
    public interface ICorrecaoService
    {
        Task<ApiProvaOnline.Application.DTOs.Responses.ResultadoParcialDTO> CalcularPontuacaoAsync(int alunoId, int provaId);
        Task CorrigirProvaAsync(int alunoId, int provaId);
        Task<IEnumerable<AlunoPontuacaoDTO>> ListarPontuacoesProvaAsync(int provaId);
    }
}


