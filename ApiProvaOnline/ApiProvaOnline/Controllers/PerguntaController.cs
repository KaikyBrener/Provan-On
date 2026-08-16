using ApiProvaOnline.Models;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "aluno,admin,professor")]
    public class PerguntaController : ControllerBase
    {
        private readonly PerguntaService _service;
        private readonly ProvaService _provaService;

        public PerguntaController(PerguntaService service, ProvaService provaService)
        {
            _service = service;
            _provaService = provaService;
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

        [HttpPost]
        [Authorize(Roles = "admin,professor")]
        public async Task<IActionResult> CadastrarPergunta([FromBody] Pergunta pergunta)
        {
            var erro = await ValidarPosseDaProvaAsync(pergunta.IdProva);
            if (erro != null) return erro;

            await _service.CadastrarPerguntaAsync(pergunta);
            return Created("", pergunta);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin,professor")]
        public async Task<IActionResult> RemoverPergunta(int id)
        {
            var pergunta = await _service.BuscarPerguntaAsync(id);
            if (pergunta == null)
                return NotFound("Pergunta não encontrada");

            var erro = await ValidarPosseDaProvaAsync(pergunta.IdProva);
            if (erro != null) return erro;

            var resultado = await _service.RemoverPerguntaAsync(id);
            if (resultado)
                return Ok("Pergunta removida com sucesso");

            return NotFound("Pergunta não encontrada");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,professor")]
        public async Task<IActionResult> BuscarPergunta(int id)
        {
            var pergunta = await _service.BuscarPerguntaAsync(id);
            if (pergunta == null) return NotFound();

            var erro = await ValidarPosseDaProvaAsync(pergunta.IdProva);
            if (erro != null) return erro;

            return Ok(pergunta);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListarPergunta()
        {
            var perguntas = await _service.ListarPerguntaAsync();
            return Ok(perguntas);
        }

        [HttpGet("Prova/{provaId}")]
        [Authorize(Roles = "aluno,admin,professor")]
        public async Task<IActionResult> ListarPorProva(int provaId)
        {
            if (User.IsInRole("aluno"))
            {
                if (!int.TryParse(User.FindFirstValue("ProvaId"), out var tokenProvaId))
                    return Unauthorized(new { message = "Token de sessão inválido." });

                if (provaId != tokenProvaId)
                    return Forbid();
            }
            else
            {
                var erro = await ValidarPosseDaProvaAsync(provaId);
                if (erro != null) return erro;
            }

            var perguntas = await _service.ListarPorProvaAsync(provaId);
            return Ok(perguntas);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,professor")]
        public async Task<IActionResult> AtualizarPergunta(int id, [FromBody] Pergunta pergunta)
        {
            var existente = await _service.BuscarPerguntaAsync(id);
            if (existente == null) return NotFound();

            var erro = await ValidarPosseDaProvaAsync(existente.IdProva);
            if (erro != null) return erro;

            if (User.IsInRole("professor") && pergunta.IdProva != existente.IdProva)
            {
                var erroNovaProva = await ValidarPosseDaProvaAsync(pergunta.IdProva);
                if (erroNovaProva != null) return erroNovaProva;
            }

            pergunta.Id = id;
            var result = await _service.AtualizarPerguntaAsync(pergunta);
            if (!result) return NotFound();

            return Ok(pergunta);
        }
    }
}
