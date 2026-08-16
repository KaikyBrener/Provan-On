using ApiProvaOnline.Models;
using ApiProvaOnline.Application.Services;
using ApiProvaOnline.Application.DTOs.Requests;
using ApiProvaOnline.Application.DTOs.Responses;
using ApiProvaOnline.Infrastructure.Security;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

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
        private readonly TokenService _tokenService;
        private readonly AutoMapper.IMapper _mapper;

        public ProvaController(ProvaService service, AlunoService alunoService,
            TokenService tokenService, AutoMapper.IMapper mapper)
        {
            _service = service;
            _alunoService = alunoService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarProva([FromBody] Prova prova)
        {
            if (prova.DataHoraInicio >= prova.DataHoraFim)
                return BadRequest(new { message = "A data/hora de início deve ser anterior à data/hora de fim." });

            // Professor só pode criar prova para si mesmo
            if (User.IsInRole("professor"))
            {
                if (!int.TryParse(User.FindFirstValue("Id"), out var profId))
                    return Unauthorized();
                prova.IdProfessor = profId;
            }

            await _service.CadastrarProvaAsync(prova);
            var dto = _mapper.Map<ProvaDTO>(prova);
            return Created("", dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverProva(int id)
        {
            if (User.IsInRole("professor"))
            {
                var prova = await _service.BuscarProvaAsync(id);
                if (prova == null) return NotFound();
                if (!int.TryParse(User.FindFirstValue("Id"), out var professorId) || prova.IdProfessor != professorId)
                    return Forbid();
            }

            bool resultado = await _service.RemoverProvaAsync(id);
            if (resultado) return Ok(resultado);
            return NotFound();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarProva(int id)
        {
            var prova = await _service.BuscarProvaAsync(id);
            if (prova == null) return NotFound();
            var dto = _mapper.Map<ProvaDTO>(prova);
            
            if (!User.Identity.IsAuthenticated || User.IsInRole("aluno"))
            {
                dto.Codigo = ""; // Não expor o código de acesso
            }

            return Ok(dto);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
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

            if (prova.DataHoraInicio >= prova.DataHoraFim)
                return BadRequest(new { message = "A data/hora de início deve ser anterior à data/hora de fim." });

            if (User.IsInRole("professor"))
            {
                var existente = await _service.BuscarProvaAsync(id);
                if (existente == null) return NotFound();
                if (!int.TryParse(User.FindFirstValue("Id"), out var professorId) || existente.IdProfessor != professorId)
                    return Forbid();
                prova.IdProfessor = professorId; // Não permite trocar o professor dono
            }

            var result = await _service.AtualizarProvaAsync(prova);
            if (!result) return NotFound();
            var dto = _mapper.Map<ProvaDTO>(prova);
            return Ok(dto);
        }

        [HttpGet("Professor/{professorId}")]
        public async Task<IActionResult> ListarProvasProfessor(int professorId)
        {
            // Professor só pode listar suas próprias provas
            if (User.IsInRole("professor"))
            {
                if (!int.TryParse(User.FindFirstValue("Id"), out var jwtProfId) || jwtProfId != professorId)
                    return Forbid();
            }

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
        [EnableRateLimiting("entrar-prova")]
        public async Task<IActionResult> Entrar([FromBody] EntrarProvaRequest request)
        {
            var prova = await _service.BuscarProvaPorCodigoAsync(request.CodigoProva);
            if (prova == null) return NotFound(new { message = "Código de prova inválido." });

            var agora = DateTime.Now;
            if (agora < prova.DataHoraInicio || agora > prova.DataHoraFim)
                return BadRequest(new { message = "A prova não está no período de realização." });

            var aluno = await _alunoService.BuscarPorNomeDataNascimentoProvaAsync(
                request.Nome, request.DataNascimento, prova.Id);

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

            // Gera token JWT de sessão para o aluno (vinculado ao alunoId e provaId)
            var token = _tokenService.GenerateTokenAluno(aluno.Id, prova.Id, aluno.Nome);

            return Ok(new
            {
                alunoId = aluno.Id,
                provaId = prova.Id,
                titulo = prova.Titulo,
                totalQuestoes = prova.PerguntasObjetivas.Count,
                token // JWT de sessão do aluno
            });
        }
    }
}
