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
    public class AlunoRespostaController : ControllerBase
    {
        private readonly AlunoRespostaService _service;

        public AlunoRespostaController(AlunoRespostaService service)
        {
            _service = service;
        }

        [HttpPost("CadastrarAlunoResposta")]
        public async Task<IActionResult> CadastrarAlunoResposta([FromBody] AlunoResposta alunoResposta)
        {
            if (!int.TryParse(User.FindFirstValue("AlunoId"), out var tokenAlunoId) ||
                !int.TryParse(User.FindFirstValue("ProvaId"), out var tokenProvaId))
                return Unauthorized(new { message = "Token de aluno inválido ou ausente." });

            if (tokenAlunoId != alunoResposta.IdAluno)
                return Forbid();

            var jaRespondeu = await _service.ExisteRespostaAsync(alunoResposta.IdAluno, alunoResposta.IdPergunta);
            if (jaRespondeu)
                return Conflict(new { message = "O aluno já respondeu esta questão." });

            try
            {
                await _service.CadastrarAlunoRespostaAsync(alunoResposta);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

            return Ok(alunoResposta);
        }

        [HttpDelete("RemoverAlunoResposta/{id}")]
        public async Task<IActionResult> RemoverAlunoResposta(int id)
        {
            bool resultado = await _service.RemoverAlunoRespostaAsync(id);
            if (resultado)
                return Ok(resultado);
            else
                return NotFound("Resposta não encontrada");
        }

        [HttpGet("BuscarAlunoResposta/{id}")]
        public async Task<IActionResult> BuscarAlunoResposta(int id)
        {
            var resposta = await _service.BuscarAlunoRespostaAsync(id);
            if (resposta == null) return NotFound();
            return Ok(resposta);
        }

        [HttpPut("AtualizarAlunoResposta/{id}")]
        public async Task<IActionResult> AtualizarAlunoResposta(int id, [FromBody] AlunoResposta alunoResposta)
        {
            alunoResposta.Id = id;
            bool resultado = await _service.AtualizarAlunoRespostaAsync(alunoResposta);
            if (resultado)
                return Ok(resultado);
            else
                return NotFound("Resposta não encontrada");
        }
    }
}
