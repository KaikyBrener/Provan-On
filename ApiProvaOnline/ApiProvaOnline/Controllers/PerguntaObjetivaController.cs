using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin,professor")]
    public class PerguntaObjetivaController : ControllerBase
    {
        private readonly PerguntaObjetivaService _service;
        private readonly ProvaService _provaService;

        public PerguntaObjetivaController(PerguntaObjetivaService service, ProvaService provaService)
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
        public async Task<IActionResult> CadastrarPerguntaObjetiva([FromBody] PerguntaObjetiva perguntaObjetiva)
        {
            var erro = await ValidarPosseDaProvaAsync(perguntaObjetiva.IdProva);
            if (erro != null) return erro;

            await _service.CadastrarPerguntaObjetivaAsync(perguntaObjetiva);
            return Created("", perguntaObjetiva);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverPerguntaObjetiva(int id)
        {
            var pergunta = await _service.BuscarPerguntaObjetivaAsync(id);
            if (pergunta == null)
                return NotFound("PerguntaObjetiva não encontrada");

            var erro = await ValidarPosseDaProvaAsync(pergunta.IdProva);
            if (erro != null) return erro;

            var resultado = await _service.RemoverPerguntaObjetivaAsync(id);
            if (resultado) return Ok("PerguntaObjetiva removida com sucesso");

            return NotFound("PerguntaObjetiva não encontrada");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPerguntaObjetiva(int id)
        {
            var pergunta = await _service.BuscarPerguntaObjetivaAsync(id);
            if (pergunta == null) return NotFound();

            var erro = await ValidarPosseDaProvaAsync(pergunta.IdProva);
            if (erro != null) return erro;

            return Ok(pergunta);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListarPerguntasObjetivas()
        {
            var perguntas = await _service.ListarPerguntasObjetivasAsync();
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
            var dto = perguntas.Select(p => new ListarPerguntarProvaDTO
            {
                id = p.Id,
                titulo = p.Titulo,
                descricao = p.Descricao,
                enunciado = p.Descricao,
                opcaoa = p.OpcaoA,
                opcaob = p.OpcaoB,
                opcaoc = p.OpcaoC,
                opcaod = p.OpcaoD,
                pontuacao = p.Pontuacao,
                alternativas = new List<string> { p.OpcaoA, p.OpcaoB, p.OpcaoC, p.OpcaoD }
            });

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPerguntaObjetiva(
            int id, [FromBody] PerguntaObjetiva perguntaObjetiva)
        {
            var existente = await _service.BuscarPerguntaObjetivaAsync(id);
            if (existente == null) return NotFound();

            var erro = await ValidarPosseDaProvaAsync(existente.IdProva);
            if (erro != null) return erro;

            if (User.IsInRole("professor") && perguntaObjetiva.IdProva != existente.IdProva)
            {
                var erroNovaProva = await ValidarPosseDaProvaAsync(perguntaObjetiva.IdProva);
                if (erroNovaProva != null) return erroNovaProva;
            }

            perguntaObjetiva.Id = id;
            var result = await _service.AtualizarPerguntaObjetivaAsync(perguntaObjetiva);
            if (!result) return NotFound();

            return Ok(perguntaObjetiva);
        }
    }
}
