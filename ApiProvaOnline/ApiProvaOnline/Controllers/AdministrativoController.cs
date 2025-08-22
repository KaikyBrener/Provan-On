using ApiProvaOnline.Model;
using ApiProvaOnline.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiProvaOnline.Controllers
{

    [ApiController]
    [Route("[controller]")]


    public class AdministrativoController : Controller
    {

        [HttpPost("CadastrarAdministrativo")]
        public IActionResult CadastrarAdministrativo([FromBody] Administrativo administrativo)
        {
            try
            {

                AdministrativoService service = new AdministrativoService();

                service.CadastrarAdministrativo(administrativo);

                return Ok(administrativo);

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
            }
        }

        [HttpDelete ("RemoverAdministrativo")]

        public IActionResult RemoverAdministrativo(int Id)
        {

            try
            {

                AdministrativoService service =new AdministrativoService();

                bool Resultado = service.RemoverAdministrativo(Id);

                if (Resultado)
                {
                    return Ok("Alteração realizada com sucesso!");
                }
                else
                {
                    return NotFound("Administrador não encontrado.");
                }

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado:" + ex.Message);

            }

        }


        [HttpGet("BuscarAdministrativo")]

        public IActionResult BuscarAdministrativo(int Id)
        {

            try
            {

                AdministrativoService service = new AdministrativoService();

                Administrativo administrativo = service.BuscarAdministrativo(Id);

                return Ok(administrativo);


            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
            }

        }

        [HttpGet("ListarAdministrativo")]

        public IActionResult ListarAdministrativo()
        {

            try
            {

                AdministrativoService service = new AdministrativoService();

                List<Administrativo> administrativo = service.ListarAdministrativo();

                return Ok(administrativo);


            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
            }

        }

        [HttpPut("AtualizarAdministrativo")]

        public IActionResult AtualizarAdministrativo([FromBody] Administrativo administrativo)
        {

            try
            {

                AdministrativoService service = new AdministrativoService();

                service.AtualizarAdministrativo(administrativo);

                return Ok(administrativo);


            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
            }

        }

    }
}
