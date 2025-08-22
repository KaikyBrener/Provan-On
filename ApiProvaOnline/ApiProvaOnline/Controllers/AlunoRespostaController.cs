using ApiProvaOnline.Model;
using ApiProvaOnline.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiProvaOnline.Controllers
{
    public class AlunoRespostaController : Controller
    {
        [HttpPost("CadastrarAlunoResposta")]
        public IActionResult CadastrarAlunoResposta([FromBody] AlunoResposta alunoResposta)
        {
            try
            {
                AlunoRespostaService service = new AlunoRespostaService();

                service.CadastrarAlunoResposta(alunoResposta);

                return Ok(alunoResposta);

            }
            catch (Exception ex)
            {
                
                return BadRequest("Erro inesperado: " + ex.Message);

            }
 
        }

        [HttpDelete("RemoverAlunoResposta")]

        public IActionResult RemoverAlunoResposta(int id)
        {
            try
            {
                AlunoRespostaService service = new AlunoRespostaService();

                bool Resultado = service.RemoverAlunoResposta(id);

                if (Resultado)
                {
                    return Ok(Resultado);

                }
                else
                { 
                
                    return NotFound("Resposta não encontrada");
                
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex.Message);
            }
        }

        [HttpGet("BuscarAlunoResposta")]

        public IActionResult BuscarAlunoResposta(int id)
        {
            try
            {
                AlunoRespostaService service = new AlunoRespostaService();

                AlunoResposta alunoResposta = service.BuscarAlunoResposta(id);

                return Ok(alunoResposta);

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
                
            }
            
        
        }

        [HttpPut("AtualizarAlunoResposta")]

        public IActionResult AtualizarAlunoResposta([FromBody] AlunoResposta alunoResposta)
        {

            try
            {
                AlunoRespostaService service = new AlunoRespostaService();

                bool Resultado = service.AtualizarAlunoResposta(alunoResposta);

                if (Resultado)
                {

                    return Ok(Resultado);

                }
                else
                {

                    return NotFound("Resposta não encontrada");

                }

            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex.Message);
            }
          
        
        
        }
    
    }
}
