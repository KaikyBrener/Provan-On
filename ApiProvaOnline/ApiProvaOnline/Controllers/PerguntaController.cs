using ApiProvaOnline.Models;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin,professor")]
    public class PerguntaController : ControllerBase
    {
        private readonly PerguntaService _service;

        public PerguntaController(PerguntaService service)
        {
            _service = service;
        }

        // POST: api/Pergunta
        [HttpPost]
        public async Task<IActionResult> CadastrarPergunta([FromBody] Pergunta pergunta)
        {
            await _service.CadastrarPerguntaAsync(pergunta);
            return Created("", pergunta);
        }

        // DELETE: api/Pergunta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverPergunta(int id)
        {
            bool resultado = await _service.RemoverPerguntaAsync(id);
            if (resultado)
                return Ok("Pergunta removida com sucesso");
            else
                return NotFound("Pergunta não encontrada");
        }

        // GET: api/Pergunta/5
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPergunta(int id)
        {
            var pergunta = await _service.BuscarPerguntaAsync(id);
            if (pergunta == null) return NotFound();
            return Ok(pergunta);
        }

        // GET: api/Pergunta
        [HttpGet]
        public async Task<IActionResult> ListarPergunta()
        {
            var perguntas = await _service.ListarPerguntaAsync();
            return Ok(perguntas);
        }

        // GET: api/Pergunta/Prova/5
        [HttpGet("Prova/{provaId}")]
        public async Task<IActionResult> ListarPorProva(int provaId)
        {
            var perguntas = await _service.ListarPorProvaAsync(provaId);
            return Ok(perguntas);
        }

        // PUT: api/Pergunta/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPergunta(int id, [FromBody] Pergunta pergunta)
        {
            pergunta.Id = id;
            var result = await _service.AtualizarPerguntaAsync(pergunta);
            if (!result) return NotFound();
            return Ok(pergunta);
        }
    }
}
