using ApiProvaOnline.Model;
using ApiProvaOnline.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiProvaOnline.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class AlunoRespostaObjetivaController : Controller
    {

        [HttpPost("CadastrarAlunoRespostaObjetiva")]
        public IActionResult CadastrarAlunoRespostaObjetiva([FromBody] AlunoRespostaObjetiva alunoRespostaObjetiva)

        {
            try
            {

                AlunoRespostaObjetivaService service = new AlunoRespostaObjetivaService();

                service.CadastrarAlunoRespostaObjetiva(alunoRespostaObjetiva);

                return Ok(alunoRespostaObjetiva);

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
            }
        }

        [HttpDelete("RemoverAlunoRespostaObjetivo")]

        public IActionResult RemoverAlunoRespostaObjetiva(int Id)
        {

            try
            {

                AlunoRespostaObjetivaService service = new AlunoRespostaObjetivaService();

                bool Resultado = service.RemoverAlunoRespostaObjetiva(Id);

                if (Resultado)
                {
                    return Ok("Alteração realizada com sucesso!");
                }
                else
                {
                    return NotFound("Aluno não encontrado.");
                }

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado:" + ex.Message);

            }

        }


        [HttpGet("BuscarAlunoRespostaObjetiva")]

        public IActionResult BuscarAlunoRespostaObjetiva(int Id)
        {

            try
            {

                AlunoRespostaObjetivaService service = new AlunoRespostaObjetivaService();

                AlunoRespostaObjetiva alunoRespostaObjetiva = service.BuscarAlunoRespostaObjetiva(Id);

                return Ok(alunoRespostaObjetiva);


            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
            }

        }

        [HttpPut("AtualizarAlunoRespostaObjetiva")]

        public IActionResult AtualizarAlunoRespostaObjetiva([FromBody] AlunoRespostaObjetiva alunoRespostaObjetiva)
        {

            try
            {

                AlunoRespostaObjetiva service = new AlunoRespostaObjetiva();

                service.AtualizarAlunoRespostaObjetiva(alunoRespostaObjetiva);

                return Ok(alunoRespostaObjetiva);


            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex.Message);
            }

        }

    }
}
