using ApiProvaOnline.Model;
using ApiProvaOnline.Service;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ApiProvaOnline.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProfessorController : Controller
    {
        private readonly ProfessorService _service = new ProfessorService();

        [HttpPost("LoginProfessor")]
        public IActionResult Login([FromBody] Model.LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            {
                return BadRequest(new { mensagem = "Email e senha são obrigatórios." });
            }

            var professor = _service.LoginProfessor(request.Email, request.Senha);

            if (professor == null)
            {
                return Unauthorized(new { mensagem = "Credenciais inválidas." });
            }

            // Opcional: Remover a senha do retorno
            professor.Senha = null;

            return Ok(new
            {
                sucesso = true,
                professor = professor
            });
        }



        [HttpPost("CadastrarProfessor")]
        public IActionResult CadastrarProfessor([FromBody] Professor professor)
        {
            try
            {
                ProfessorService service = new ProfessorService();

                service.CadastrarProfessor(professor);

                return Ok(professor);


            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado:" + ex.Message);

            }
        }

        [HttpDelete("RemoverProfessor")]

        public IActionResult RemoverProfessor(int Id)
        {
            try
            {
                ProfessorService service = new ProfessorService();

                return Ok(service.RemoverProfessor(Id));



            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado:" + ex.Message);

            }


        }


        [HttpGet("BuscarProfessor")]

        public IActionResult BuscarProfessor(int Id)
        {
            try
            {
                ProfessorService service = new ProfessorService();

                Professor professor = service.BuscarProfessor(Id);

                return Ok(professor);

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado:" + ex.Message);

            }

        }



        [HttpGet("ListarProfessor")]

        public IActionResult ListarProfessor()
        {
            try
            {
                ProfessorService service = new ProfessorService();

                List<Professor> professor = service.ListarProfessores();

                return Ok(professor);
            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);

            }


        }

        [HttpPut("AtualizarProfessor")]

        public IActionResult AtualizarProfessor([FromBody]Professor professor)
        {

            try
            {
                ProfessorService service = new ProfessorService();

                service.AtualizarProfessor(professor);

                return Ok(professor);

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
            }
        
        
        }

    }

}
     
