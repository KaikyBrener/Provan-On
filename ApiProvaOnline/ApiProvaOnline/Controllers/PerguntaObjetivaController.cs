using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin,professor")]
    public class PerguntaObjetivaController : ControllerBase
    {
        private readonly PerguntaObjetivaService _service;

        public PerguntaObjetivaController(PerguntaObjetivaService service)
        {
            _service = service;
        }

        // POST: api/PerguntaObjetiva
        [HttpPost]
        public async Task<IActionResult> CadastrarPerguntaObjetiva([FromBody] PerguntaObjetiva perguntaObjetiva)
        {
            await _service.CadastrarPerguntaObjetivaAsync(perguntaObjetiva);
            return Created("", perguntaObjetiva);
        }

        // DELETE: api/PerguntaObjetiva/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverPerguntaObjetiva(int id)
        {
            bool resultado = await _service.RemoverPerguntaObjetivaAsync(id);
            if (resultado)
                return Ok("PerguntaObjetiva Removida com sucesso");
            else
                return NotFound("PerguntaObjetiva Não encontrada");
        }

        // GET: api/PerguntaObjetiva/5
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPerguntaObjetiva(int id)
        {
            var pergunta = await _service.BuscarPerguntaObjetivaAsync(id);
            if (pergunta == null) return NotFound();
            return Ok(pergunta);
        }

        // GET: api/PerguntaObjetiva
        [HttpGet]
        public async Task<IActionResult> ListarPerguntasObjetivas()
        {
            var perguntas = await _service.ListarPerguntasObjetivasAsync();
            return Ok(perguntas);
        }

        // GET: api/PerguntaObjetiva/Prova/5
        [HttpGet("Prova/{provaId}")]
        public async Task<IActionResult> ListarPorProva(int provaId)
        {
            var perguntas = await _service.ListarPorProvaAsync(provaId);
            var dto = perguntas.Select(p => new ListarPerguntarProvaDTO
            {
                id = p.Id,
                titulo = p.Titulo,
                descricao = p.Descricao,
                opcaoa = p.OpcaoA,
                opcaob = p.OpcaoB,
                opcaoc = p.OpcaoC,
                opcaod = p.OpcaoD,
                pontuacao = p.Pontuacao
            });
            return Ok(dto);
        }

        // PUT: api/PerguntaObjetiva/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPerguntaObjetiva(int id, [FromBody] PerguntaObjetiva perguntaObjetiva)
        {
            perguntaObjetiva.Id = id;
            var result = await _service.AtualizarPerguntaObjetivaAsync(perguntaObjetiva);
            if (!result) return NotFound();
            return Ok(perguntaObjetiva);
        }
    }
}
