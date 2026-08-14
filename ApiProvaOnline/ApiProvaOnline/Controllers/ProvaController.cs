using ApiProvaOnline.Models;
using ApiProvaOnline.Application.Services;
using ApiProvaOnline.Application.DTOs.Requests;
using ApiProvaOnline.Application.DTOs.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiProvaOnline.Controllers
{
    [EnableCors("PermitirLocalhost")]
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin,professor")]
    public class ProvaController : ControllerBase
    {
        private readonly ProvaService _service;
        private readonly AlunoService _alunoService;
        private readonly AutoMapper.IMapper _mapper;

        public ProvaController(ProvaService service, AlunoService alunoService, AutoMapper.IMapper mapper)
        {
            _service = service;
            _alunoService = alunoService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarProva([FromBody] Prova prova)
        {
            await _service.CadastrarProvaAsync(prova);
            var dto = _mapper.Map<ProvaDTO>(prova);
            return Created("", dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverProva(int id)
        {
            bool resultado = await _service.RemoverProvaAsync(id);
            if (resultado) return Ok(resultado);
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarProva(int id)
        {
            var prova = await _service.BuscarProvaAsync(id);
            if (prova == null) return NotFound();
            var dto = _mapper.Map<ProvaDTO>(prova);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> ListarProva()
        {
            var provas = await _service.ListarProvaAsync();
            var dtos = _mapper.Map<IEnumerable<ProvaDTO>>(provas);
            return Ok(dtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProva(int id, [FromBody] Prova prova)
        {
            prova.Id = id;
            var result = await _service.AtualizarProvaAsync(prova);
            if (!result) return NotFound();
            var dto = _mapper.Map<ProvaDTO>(prova);
            return Ok(dto);
        }

        [HttpGet("Professor/{professorId}")]
        public async Task<IActionResult> ListarProvasProfessor(int professorId)
        {
            var provas = await _service.ListarProvasProfessorAsync(professorId);
            var dtos = _mapper.Map<IEnumerable<ProvaDTO>>(provas);
            return Ok(dtos);
        }

        [HttpGet("Codigo/{codigo}")]
        public async Task<IActionResult> BuscarProvaPorCodigo(string codigo)
        {
            var prova = await _service.BuscarProvaPorCodigoAsync(codigo);
            if (prova == null) return NotFound(new { message = "Código de acesso inválido." });
            var dto = _mapper.Map<ProvaDTO>(prova);
            return Ok(dto);
        }

        [HttpPost("Entrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Entrar([FromBody] EntrarProvaRequest request)
        {
            var prova = await _service.BuscarProvaPorCodigoAsync(request.CodigoProva);
            if (prova == null) return NotFound(new { message = "Código de prova inválido." });

            var aluno = await _alunoService.BuscarPorNomeDataNascimentoProvaAsync(request.Nome, request.DataNascimento, prova.Id);

            if (aluno == null)
            {
                aluno = new Aluno
                {
                    Nome = request.Nome,
                    DataNascimento = request.DataNascimento,
                    IdProva = prova.Id,
                    DataHoraRegistro = DateTime.Now
                };
                await _alunoService.CadastrarAlunoAsync(aluno);
            }

            return Ok(new
            {
                alunoId = aluno.Id,
                provaId = prova.Id,
                titulo = prova.Titulo
            });
        }
    }
}
