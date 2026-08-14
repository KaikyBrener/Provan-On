using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Models;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlunoRespostaObjetivaController : ControllerBase
    {
        private readonly AlunoRespostaObjetivaService _service;
        private readonly ICorrecaoService _correcaoService;

        public AlunoRespostaObjetivaController(
            AlunoRespostaObjetivaService service,
            ICorrecaoService correcaoService)
        {
            _service = service;
            _correcaoService = correcaoService;
        }

        [HttpPost("CadastrarAlunoRespostaObjetiva")]
        public async Task<IActionResult> CadastrarAlunoRespostaObjetiva([FromBody] AlunoRespostaObjetiva alunoRespostaObjetiva)
        {
            await _service.CadastrarAlunoRespostaObjetivaAsync(alunoRespostaObjetiva);
            return Ok(alunoRespostaObjetiva);
        }

        [HttpDelete("RemoverAlunoRespostaObjetivo/{id}")]
        public async Task<IActionResult> RemoverAlunoRespostaObjetiva(int id)
        {
            bool resultado = await _service.RemoverAlunoRespostaObjetivaAsync(id);
            if (resultado)
                return Ok("Alteração realizada com sucesso!");
            else
                return NotFound("Aluno não encontrado.");
        }

        [HttpGet("BuscarAlunoRespostaObjetiva/{id}")]
        public async Task<IActionResult> BuscarAlunoRespostaObjetiva(int id)
        {
            var resposta = await _service.BuscarAlunoRespostaObjetivaAsync(id);
            if (resposta == null) return NotFound();
            return Ok(resposta);
        }

        [HttpPut("AtualizarAlunoRespostaObjetiva/{id}")]
        public async Task<IActionResult> AtualizarAlunoRespostaObjetiva(int id, [FromBody] AlunoRespostaObjetiva alunoRespostaObjetiva)
        {
            alunoRespostaObjetiva.Id = id;
            await _service.AtualizarAlunoRespostaObjetivaAsync(alunoRespostaObjetiva);
            return Ok(alunoRespostaObjetiva);
        }

        [HttpPost("CorrigirProva")]
        public async Task<IActionResult> CorrigirProva(int alunoId, int provaId)
        {
            await _correcaoService.CorrigirProvaAsync(alunoId, provaId);
            return Ok("Prova corrigida com sucesso.");
        }

        [HttpGet("PontuacaoAluno")]
        public async Task<IActionResult> CalcularPontuacao(int alunoId, int provaId)
        {
            var pontuacao = await _correcaoService.CalcularPontuacaoAsync(alunoId, provaId);
            return Ok(pontuacao);
        }

        [HttpGet("ListarPontuacoesProva/{provaId}")]
        public async Task<IActionResult> ListarPontuacoesProva(int provaId)
        {
            var pontuacoes = await _correcaoService.ListarPontuacoesProvaAsync(provaId);
            return Ok(pontuacoes);
        }
    }
}
