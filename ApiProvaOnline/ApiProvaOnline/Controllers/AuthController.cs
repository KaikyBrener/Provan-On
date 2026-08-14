using Microsoft.AspNetCore.Mvc;
using ApiProvaOnline.Application.Services;
using ApiProvaOnline.Infrastructure.Security;
using ApiProvaOnline.Application.DTOs.Requests;
using ApiProvaOnline.Application.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ProfessorService _professorService;
        private readonly AdministrativoService _adminService;
        private readonly TokenService _tokenService;

        public AuthController(ProfessorService professorService, AdministrativoService adminService, TokenService tokenService)
        {
            _professorService = professorService;
            _adminService = adminService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Tenta logar como professor
            var professor = await _professorService.LoginProfessorAsync(request.Email, request.Senha);
            if (professor != null)
            {
                var token = _tokenService.GenerateTokenProfessor(professor);
                return Ok(new LoginResponse
                {
                    Token = token,
                    Usuario = new { id = professor.Id, nome = professor.Nome, email = professor.Email, role = "professor" }
                });
            }

            // Tenta logar como admin
            var admin = await _adminService.LoginAdministrativoAsync(request.Email, request.Senha);
            if (admin != null)
            {
                var token = _tokenService.GenerateTokenAdministrativo(admin);
                return Ok(new LoginResponse
                {
                    Token = token,
                    Usuario = new { id = admin.Id, nome = admin.Nome, email = admin.Email, role = "admin" }
                });
            }

            return Unauthorized(new { message = "Credenciais inválidas" });
        }
    }
}
