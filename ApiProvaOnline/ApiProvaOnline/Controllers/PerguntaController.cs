using ApiProvaOnline.Model;
using ApiProvaOnline.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiProvaOnline.Controllers
{
    public class PerguntaController : Controller
    {

        [HttpPost("CadastrarPergunta")]
        public IActionResult CadastrarPergunta([FromBody]Pergunta pergunta)
        {
            try
            {
                PerguntaService service = new PerguntaService();

                service.CadastrarPergunta(pergunta);

                return Ok(pergunta);

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex);
            }

        }

        [HttpDelete("DeletarPergunta")]
        public IActionResult RemoverPergunta(int Id)
        {
            try
            {
                PerguntaService service = new PerguntaService();

                bool Resultado = service.RemoverPergunta(Id);

                if (Resultado == true)
                {

                    return Ok("Pergunta removida com sucesso");


                }
                else
                { 
               
                    return NotFound("Pergunta não encontrada");
                }

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex);

            }
        
        
        }

        [HttpGet("BuscarPergunta")]

        public IActionResult BuscarPergunta(int id)
        {
            try
            {
                PerguntaService service = new PerguntaService();

                Pergunta pergunta = service.BuscarPergunta(id);

                return Ok(pergunta);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex);
            }
       
        }

        [HttpGet("ListarPergunta")]

        public IActionResult ListarPergunta()
        {
            try
            {
                PerguntaService service = new PerguntaService();

                List<Pergunta> pergunta = service.ListarPergunta();

                return Ok(pergunta);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex);
            }
    
        
        }

        [HttpPut("AtualizarPergunta")]

        public IActionResult AtualizarPergunta([FromBody] Pergunta pergunta)
        {
            try
            {
                PerguntaService service = new PerguntaService();

                service.AtualizarPergunta(pergunta);

                return Ok(pergunta);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex);
            }
        }
    }

}
