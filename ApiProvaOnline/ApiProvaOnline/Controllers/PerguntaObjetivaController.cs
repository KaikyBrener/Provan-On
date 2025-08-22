using ApiProvaOnline.Model;
using ApiProvaOnline.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiProvaOnline.Controllers
{
    public class PerguntaObjetivaController : Controller
    {
        [HttpPost("CadastrarPerguntaObjetiva")]
        public IActionResult CadastrarPerguntaObjetiva([FromBody]PerguntaObjetiva perguntaObjetiva)
        {
            try
            {
                PerguntaObjetivaService service = new PerguntaObjetivaService();

                service.CadastrarPerguntaObjetiva(perguntaObjetiva);

                return Ok(perguntaObjetiva);


            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);

            }

        }

        [HttpDelete("DeletarPerguntaObjetiva")]

        public IActionResult RemoverPerguntaObjetiva(int id)
        {
            try
            {
                PerguntaObjetivaService service = new PerguntaObjetivaService();

                bool resultado = service.RemoverPerguntaObjetiva(id);

                if (resultado == true)
                {

                    return Ok("PerguntaObjetiva Removida com sucesso");

                }
                else
                {
                    return NotFound("PerguntaObjetiva Não encontrada");

                }


            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex.Message);
            }
        }


        [HttpGet("BuscarPerguntaObjetiva")]

        public IActionResult BuscarPerguntaObjetiva(int id)
        {
            try
            {
                PerguntaObjetivaService service = new PerguntaObjetivaService();

                PerguntaObjetiva perguntaobjetiva = service.BuscarPerguntaObjetiva(id);

                return Ok(perguntaobjetiva);


            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);

            }
        }

        [HttpPut("AtualizarPerguntaObjetiva")]

        public IActionResult AtualizarPerguntaObjetiva([FromBody]PerguntaObjetiva perguntaObjetiva)
        {
            try
            {
                PerguntaObjetivaService service = new PerguntaObjetivaService();

                service.AtualizarPerguntaObjetiva(perguntaObjetiva);

                return Ok(perguntaObjetiva);
            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
            }
        
        
        }
    }
}
