using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Application.DTOs.Responses;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin,professor")]
    public class AlunoController : ControllerBase
    {
        private readonly AlunoService _service;
        private readonly AutoMapper.IMapper _mapper;

        public AlunoController(AlunoService service, AutoMapper.IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // POST: api/Aluno
        [HttpPost]
        public async Task<IActionResult> CadastrarAluno([FromBody] Aluno aluno)
        {
            await _service.CadastrarAlunoAsync(aluno);
            var dto = _mapper.Map<AlunoDTO>(aluno);
            return Created("", dto);
        }

        // DELETE: api/Aluno/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverAluno(int id)
        {
            bool resultado = await _service.RemoverAlunoAsync(id);
            if (resultado)
                return Ok("O Aluno foi removido");
            else
                return BadRequest("O Aluno não foi encontrado");
        }

        // GET: api/Aluno/5
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarAluno(int id)
        {
            var aluno = await _service.BuscarAlunoAsync(id);
            if (aluno == null) return NotFound();
            var dto = _mapper.Map<AlunoDTO>(aluno);
            return Ok(dto);
        }

        // GET: api/Aluno
        [HttpGet]
        public async Task<IActionResult> ListarAlunos()
        {
            var alunos = await _service.ListarAlunoAsync();
            var dtos = _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
            return Ok(dtos);
        }

        // PUT: api/Aluno/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAluno(int id, [FromBody] Aluno aluno)
        {
            aluno.Id = id;
            var result = await _service.AtualizarAlunoAsync(aluno);
            if (!result) return NotFound();
            var dto = _mapper.Map<AlunoDTO>(aluno);
            return Ok(dto);
        }

        // GET: api/Aluno/5/Historico
        [HttpGet("{id}/Historico")]
        public async Task<ActionResult<IEnumerable<HistoricoAlunoDTO>>> ObterHistorico(int id)
        {
            var historico = await _service.HistoricoDeAlunoAsync(id);
            var lista = historico.ToList();
            if (lista.Count == 0)
                return NotFound("Nenhum histórico encontrado para o aluno.");
            return Ok(lista);
        }

        // GET: api/Aluno/Prova/5
        [HttpGet("Prova/{provaId}")]
        public async Task<IActionResult> ListarAlunosPorProva(int provaId)
        {
            var alunos = await _service.ListarPorProvaAsync(provaId);
            var dtos = _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
            return Ok(dtos);
        }
    }
}
