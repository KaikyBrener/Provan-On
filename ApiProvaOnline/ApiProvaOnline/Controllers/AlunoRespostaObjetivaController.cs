using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Application.DTOs.Requests;
using ApiProvaOnline.Models;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlunoRespostaObjetivaController : ControllerBase
    {
        private readonly AlunoRespostaObjetivaService _service;
        private readonly ICorrecaoService _correcaoService;
        private readonly ProvaService _provaService;
        private readonly PerguntaObjetivaService _perguntaObjetivaService;

        public AlunoRespostaObjetivaController(
            AlunoRespostaObjetivaService service,
            ICorrecaoService correcaoService,
            ProvaService provaService,
            PerguntaObjetivaService perguntaObjetivaService)
        {
            _service = service;
            _correcaoService = correcaoService;
            _provaService = provaService;
            _perguntaObjetivaService = perguntaObjetivaService;
        }

        [HttpPost("CadastrarAlunoRespostaObjetiva")]
        public async Task<IActionResult> CadastrarAlunoRespostaObjetiva(
            [FromBody] CadastrarRespostaObjetivaRequest request)
        {
            if (!int.TryParse(User.FindFirstValue("AlunoId"), out var tokenAlunoId) ||
                !int.TryParse(User.FindFirstValue("ProvaId"), out var tokenProvaId))
                return Unauthorized(new { message = "Token de aluno inválido ou ausente." });

            if (tokenAlunoId != request.AlunoId || tokenProvaId != request.ProvaId)
                return Forbid();

            if (request.AlternativaSelecionada < 0 || request.AlternativaSelecionada > 3)
                return BadRequest(new { message = "Alternativa selecionada inválida (use 0=A, 1=B, 2=C, 3=D)." });

            var prova = await _provaService.BuscarProvaAsync(tokenProvaId);
            if (prova == null)
                return NotFound(new { message = "Prova não encontrada." });

            if (DateTime.Now > prova.DataHoraFim)
                return BadRequest(new { message = "O período da prova encerrou." });

            var pergunta = await _perguntaObjetivaService.BuscarPerguntaObjetivaAsync(request.PerguntaObjetivaId);
            if (pergunta == null)
                return NotFound(new { message = "Pergunta não encontrada." });

            if (pergunta.IdProva != tokenProvaId)
                return Forbid();

            var jaRespondeu = await _service.ExisteRespostaAsync(tokenAlunoId, request.PerguntaObjetivaId);
            if (jaRespondeu)
                return Conflict(new { message = "O aluno já respondeu esta questão." });

            var respostaChar = (char)('A' + request.AlternativaSelecionada);

            var alunoRespostaObjetiva = new AlunoRespostaObjetiva
            {
                IdAluno = tokenAlunoId,
                IdPerguntaObjetiva = request.PerguntaObjetivaId,
                Resposta = respostaChar
            };

            try
            {
                await _service.CadastrarAlunoRespostaObjetivaAsync(alunoRespostaObjetiva);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

            return Ok(alunoRespostaObjetiva);
        }

        private async Task<IActionResult?> ValidarPosseDaProvaAsync(int provaId)
        {
            if (!User.IsInRole("professor")) return null;

            if (!int.TryParse(User.FindFirstValue("Id"), out var tokenProfessorId))
                return Unauthorized();

            var prova = await _provaService.BuscarProvaAsync(provaId);
            if (prova == null)
                return NotFound(new { message = "Prova não encontrada." });

            if (prova.IdProfessor != tokenProfessorId)
                return Forbid();

            return null;
        }

        private async Task<IActionResult?> ValidarPosseDaRespostaAsync(AlunoRespostaObjetiva resposta)
        {
            if (!User.IsInRole("professor")) return null;

            var pergunta = await _perguntaObjetivaService.BuscarPerguntaObjetivaAsync(resposta.IdPerguntaObjetiva);
            if (pergunta == null)
                return NotFound(new { message = "Pergunta não encontrada." });

            return await ValidarPosseDaProvaAsync(pergunta.IdProva);
        }

        [HttpDelete("RemoverAlunoRespostaObjetivo/{id}")]
        [Authorize(Roles = "admin,professor")]
        public async Task<IActionResult> RemoverAlunoRespostaObjetiva(int id)
        {
            var resposta = await _service.BuscarAlunoRespostaObjetivaAsync(id);
            if (resposta == null) return NotFound("Aluno não encontrado.");

            var erro = await ValidarPosseDaRespostaAsync(resposta);
            if (erro != null) return erro;

            var resultado = await _service.RemoverAlunoRespostaObjetivaAsync(id);
            if (resultado) return Ok("Alteração realizada com sucesso!");

            return NotFound("Aluno não encontrado.");
        }

        [HttpGet("BuscarAlunoRespostaObjetiva/{id}")]
        [Authorize(Roles = "admin,professor")]
        public async Task<IActionResult> BuscarAlunoRespostaObjetiva(int id)
        {
            var resposta = await _service.BuscarAlunoRespostaObjetivaAsync(id);
            if (resposta == null) return NotFound();

            var erro = await ValidarPosseDaRespostaAsync(resposta);
            if (erro != null) return erro;

            return Ok(resposta);
        }

        [HttpPut("AtualizarAlunoRespostaObjetiva/{id}")]
        [Authorize(Roles = "admin,professor")]
        public async Task<IActionResult> AtualizarAlunoRespostaObjetiva(
            int id, [FromBody] AlunoRespostaObjetiva alunoRespostaObjetiva)
        {
            var existente = await _service.BuscarAlunoRespostaObjetivaAsync(id);
            if (existente == null) return NotFound();

            var erro = await ValidarPosseDaRespostaAsync(existente);
            if (erro != null) return erro;

            alunoRespostaObjetiva.Id = id;
            await _service.AtualizarAlunoRespostaObjetivaAsync(alunoRespostaObjetiva);
            return Ok(alunoRespostaObjetiva);
        }

        [HttpPost("CorrigirProva")]
        public async Task<IActionResult> CorrigirProva(int alunoId, int provaId)
        {
            if (!int.TryParse(User.FindFirstValue("AlunoId"), out var tokenAlunoId) ||
                !int.TryParse(User.FindFirstValue("ProvaId"), out var tokenProvaId))
                return Unauthorized(new { message = "Token de aluno inválido ou ausente." });

            if (tokenAlunoId != alunoId || tokenProvaId != provaId)
                return Forbid();

            await _correcaoService.CorrigirProvaAsync(alunoId, provaId);
            return Ok("Prova corrigida com sucesso.");
        }

        [HttpGet("PontuacaoAluno")]
        public async Task<IActionResult> CalcularPontuacao(int alunoId, int provaId)
        {
            if (User.IsInRole("aluno"))
            {
                if (!int.TryParse(User.FindFirstValue("AlunoId"), out var tokenAlunoId) ||
                    !int.TryParse(User.FindFirstValue("ProvaId"), out var tokenProvaId) ||
                    tokenAlunoId != alunoId || tokenProvaId != provaId)
                    return Forbid();
            }
            else if (User.IsInRole("professor"))
            {
                var erro = await ValidarPosseDaProvaAsync(provaId);
                if (erro != null) return erro;
            }
            else if (!User.IsInRole("admin"))
            {
                return Forbid();
            }

            var pontuacao = await _correcaoService.CalcularPontuacaoAsync(alunoId, provaId);
            return Ok(pontuacao);
        }

        [HttpGet("ListarPontuacoesProva/{provaId}")]
        [Authorize(Roles = "admin,professor")]
        public async Task<IActionResult> ListarPontuacoesProva(int provaId)
        {
            var erro = await ValidarPosseDaProvaAsync(provaId);
            if (erro != null) return erro;

            var pontuacoes = await _correcaoService.ListarPontuacoesProvaAsync(provaId);
            return Ok(pontuacoes);
        }
    }
}
