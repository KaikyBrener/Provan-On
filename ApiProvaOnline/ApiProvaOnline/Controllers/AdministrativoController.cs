using Microsoft.AspNetCore.Authorization;
using ApiProvaOnline.Models;
using ApiProvaOnline.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AdministrativoController : ControllerBase
    {
        private readonly AdministrativoService _service;
        private readonly AutoMapper.IMapper _mapper;

        public AdministrativoController(AdministrativoService service, AutoMapper.IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAdministrativo([FromBody] Administrativo administrativo)
        {
            await _service.CadastrarAdministrativoAsync(administrativo);
            var dto = _mapper.Map<ApiProvaOnline.Application.DTOs.Responses.AdministrativoDTO>(administrativo);
            return Created("", dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverAdministrativo(int id)
        {
            bool resultado = await _service.RemoverAdministrativoAsync(id);
            if (resultado)
                return Ok("Alteração realizada com sucesso!");
            else
                return NotFound("Administrador não encontrado.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarAdministrativo(int id)
        {
            var administrativo = await _service.BuscarAdministrativoAsync(id);
            if (administrativo == null) return NotFound();
            var dto = _mapper.Map<ApiProvaOnline.Application.DTOs.Responses.AdministrativoDTO>(administrativo);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> ListarAdministrativo()
        {
            var administrativos = await _service.ListarAdministrativoAsync();
            var dtos = _mapper.Map<IEnumerable<ApiProvaOnline.Application.DTOs.Responses.AdministrativoDTO>>(administrativos);
            return Ok(dtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAdministrativo(int id, [FromBody] Administrativo administrativo)
        {
            administrativo.Id = id;
            await _service.AtualizarAdministrativoAsync(administrativo);
            var dto = _mapper.Map<ApiProvaOnline.Application.DTOs.Responses.AdministrativoDTO>(administrativo);
            return Ok(dto);
        }
    }
}
