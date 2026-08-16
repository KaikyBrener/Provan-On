using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs;
using ApiProvaOnline.Application.DTOs.Responses;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin,professor")]
    public class AlunoController : ControllerBase
    {
        private readonly AlunoService _service;
        private readonly ProvaService _provaService;
        private readonly AutoMapper.IMapper _mapper;

        public AlunoController(AlunoService service, ProvaService provaService, AutoMapper.IMapper mapper)
        {
            _service = service;
            _provaService = provaService;
            _mapper = mapper;
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
        public async Task<IActionResult> CadastrarAluno([FromBody] Aluno aluno)
        {
            var erro = await ValidarPosseDaProvaAsync(aluno.IdProva);
            if (erro != null) return erro;

            await _service.CadastrarAlunoAsync(aluno);
            var dto = _mapper.Map<AlunoDTO>(aluno);
            return Created("", dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverAluno(int id)
        {
            var aluno = await _service.BuscarAlunoAsync(id);
            if (aluno == null)
                return BadRequest("O aluno não foi encontrado");

            var erro = await ValidarPosseDaProvaAsync(aluno.IdProva);
            if (erro != null) return erro;

            var resultado = await _service.RemoverAlunoAsync(id);
            if (resultado)
                return Ok("O aluno foi removido");

            return BadRequest("O aluno não foi encontrado");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarAluno(int id)
        {
            var aluno = await _service.BuscarAlunoAsync(id);
            if (aluno == null) return NotFound();

            var erro = await ValidarPosseDaProvaAsync(aluno.IdProva);
            if (erro != null) return erro;

            var dto = _mapper.Map<AlunoDTO>(aluno);
            return Ok(dto);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListarAlunos()
        {
            var alunos = await _service.ListarAlunoAsync();
            var dtos = _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
            return Ok(dtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAluno(int id, [FromBody] Aluno aluno)
        {
            var existente = await _service.BuscarAlunoAsync(id);
            if (existente == null) return NotFound();

            var erro = await ValidarPosseDaProvaAsync(existente.IdProva);
            if (erro != null) return erro;

            if (User.IsInRole("professor") && aluno.IdProva != existente.IdProva)
            {
                var erroNovaProva = await ValidarPosseDaProvaAsync(aluno.IdProva);
                if (erroNovaProva != null) return erroNovaProva;
            }

            aluno.Id = id;
            var result = await _service.AtualizarAlunoAsync(aluno);
            if (!result) return NotFound();

            var dto = _mapper.Map<AlunoDTO>(aluno);
            return Ok(dto);
        }

        [HttpGet("{id}/Historico")]
        public async Task<IActionResult> ObterHistorico(int id)
        {
            var aluno = await _service.BuscarAlunoAsync(id);
            if (aluno == null)
                return NotFound("Nenhum histórico encontrado para o aluno.");

            var erro = await ValidarPosseDaProvaAsync(aluno.IdProva);
            if (erro != null) return erro;

            var historico = await _service.HistoricoDeAlunoAsync(id);
            var lista = historico.ToList();

            if (lista.Count == 0)
                return NotFound("Nenhum histórico encontrado para o aluno.");

            return Ok(lista);
        }

        [HttpGet("Prova/{provaId}")]
        public async Task<IActionResult> ListarAlunosPorProva(int provaId)
        {
            var erro = await ValidarPosseDaProvaAsync(provaId);
            if (erro != null) return erro;

            var alunos = await _service.ListarPorProvaAsync(provaId);
            var dtos = _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
            return Ok(dtos);
        }
    }
}
