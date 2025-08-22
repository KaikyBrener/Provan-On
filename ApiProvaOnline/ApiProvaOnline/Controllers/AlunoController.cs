using ApiProvaOnline.Model;
using ApiProvaOnline.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiProvaOnline.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunoController : Controller
    {
        [HttpPost("CadastrarAluno")]
        public IActionResult CadastrarAluno([FromBody] Aluno aluno)
        {
            try
            {
                AlunoService service = new AlunoService();

                service.CadastrarAluno(aluno);

                return Ok(aluno);

            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado:" + ex.Message);
            }

        }
        [HttpDelete("RemoverAluno")]

        public IActionResult RemoverAluno(int id)
        {
            try
            {
                AlunoService service = new AlunoService();

                bool resultado = service.RemoverAluno(id);

                if (resultado)
                {

                    return Ok("O Aluno foi removido");

                }
                else
                {
                    return BadRequest("O Aluno não foi encontrado");

                }


            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado:" + ex.Message);

            }


        }

        [HttpGet("BuscarAluno")]

        public IActionResult BuscarAluno(int id)
        {
            try
            {
                AlunoService service = new AlunoService();

                Aluno aluno = service.BuscarAluno(id);

                return Ok(aluno);

            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado:" + ex.Message);

            }

        }

        [HttpGet("ListarAlunos")]

        public IActionResult ListarAlunos()
        {

            try
            {
                AlunoService service = new AlunoService();

                List<Aluno> alunos = service.ListarAluno();

                return Ok(alunos);
            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado:" + ex.Message);

            }


        }
        [HttpPut("AtualizarAluno")]

        public IActionResult AtualizarAluno(Aluno alunos)
        {

            try
            {
                AlunoService service = new AlunoService();

                service.AtualizarAluno(alunos);

                return Ok(alunos);


            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado:" + ex.Message);
            }


        }

        [HttpGet("Historico")]
        public ActionResult<List<HistoricoAlunoDTO>> ObterHistorico(int id)
        {
            try
            {
                AlunoService service = new AlunoService();

               
                HistoricoAlunoDTO dto = new HistoricoAlunoDTO { id = id };

              
                List<HistoricoAlunoDTO> historico = service.HistoricoDeAluno(dto);

                if (historico == null || historico.Count == 0)
                {
                    return NotFound("Nenhum histórico encontrado para o aluno.");
                }

                return Ok(historico);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex.Message);
            }
        }


    }

}

