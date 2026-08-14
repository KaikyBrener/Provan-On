using ApiProvaOnline.Models;
using ApiProvaOnline.Application.DTOs.Requests;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class ProfessorController : ControllerBase
    {
        private readonly ProfessorService _service;
        private readonly AutoMapper.IMapper _mapper;

        public ProfessorController(ProfessorService service, AutoMapper.IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // POST: api/Professor
        [HttpPost]
        public async Task<IActionResult> CadastrarProfessor([FromBody] Professor professor)
        {
            await _service.CadastrarProfessorAsync(professor);
            var dto = _mapper.Map<ApiProvaOnline.Application.DTOs.Responses.ProfessorDTO>(professor);
            return Created("", dto);
        }

        // DELETE: api/Professor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverProfessor(int id)
        {
            var result = await _service.RemoverProfessorAsync(id);
            if (!result) return NotFound();
            return Ok(result);
        }

        // GET: api/Professor/5
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarProfessor(int id)
        {
            var professor = await _service.BuscarProfessorAsync(id);
            if (professor == null) return NotFound();
            var dto = _mapper.Map<ApiProvaOnline.Application.DTOs.Responses.ProfessorDTO>(professor);
            return Ok(dto);
        }

        // GET: api/Professor
        [HttpGet]
        public async Task<IActionResult> ListarProfessor()
        {
            var professores = await _service.ListarProfessoresAsync();
            var dtos = _mapper.Map<IEnumerable<ApiProvaOnline.Application.DTOs.Responses.ProfessorDTO>>(professores);
            return Ok(dtos);
        }

        // PUT: api/Professor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProfessor(int id, [FromBody] Professor professor)
        {
            // Garante que o Id no corpo é igual ao da rota
            professor.Id = id; 
            var result = await _service.AtualizarProfessorAsync(professor);
            if (!result) return NotFound();
            var dto = _mapper.Map<ApiProvaOnline.Application.DTOs.Responses.ProfessorDTO>(professor);
            return Ok(dto);
        }
    }
}
