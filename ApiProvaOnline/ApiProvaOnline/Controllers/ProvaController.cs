using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;
using ApiProvaOnline.Model;
using ApiProvaOnline.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ApiProvaOnline.Controllers
{
    [EnableCors("PermitirLocalhost")]

    public class ProvaController : Controller
    {

        [HttpPost("CadastrarProva")]
        public IActionResult CadastrarProva([FromBody] Prova prova)
        {
            try
            {
                ProvaService service = new ProvaService();

                service.CadastrarProva(prova);

                return Ok(prova);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex);
            }

        }


        [HttpDelete("RemoverProva")]
        public IActionResult RemoverProva(int Id)
        {
            try
            {
                ProvaService service = new ProvaService();

                bool Resultado = service.RemoverProva(Id);

                if (Resultado)
                {

                    return Ok(Resultado);
                }
                else
                {

                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex);

            }
        
        
        
        }




        [HttpGet("BuscarProva")]

        public IActionResult BuscarProva(int Id)
        {
            try
            {
                ProvaService service = new ProvaService();

                Prova prova = service.BuscarProva(Id);

                return Ok(prova);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex);
            }
        
        
        }

        [HttpGet("ListarProva")]

        public IActionResult ListarProva()
        {

            try
            {
                ProvaService service = new ProvaService();

                List<Prova> prova = service.ListarProva();

                return Ok(prova);
            }
            catch (Exception ex)
            {

                return BadRequest("Erro inesperado: " + ex);

            }
        
        }


        [HttpPut("AtualizarProva")]

        public IActionResult AtualizarProva([FromBody]Prova prova)
        {
            try
            {
                ProvaService service = new ProvaService();

                service.AtualizarProva(prova);

                return Ok(prova);

            }
            catch (Exception ex)
            {
                return BadRequest("Erro inesperado: " + ex);
            }
        
        
        }


        [HttpGet("ListarProvasProfessor/{professorId}")]
        public IActionResult ListarProvasProfessor(int professorId)
        {
            var provas = new List<ProvasProfessorDTO>();

            try
            {
                using var conn = DbConnection.GetConnection();
                string sql = "SELECT id, titulo, dataHoraRegistro, codigo FROM Prova WHERE idProfessor = @idProfessor";

                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idProfessor", professorId);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    provas.Add(new ProvasProfessorDTO
                    {
                        Id = reader.GetInt32("id"),
                        Titulo = reader.GetString("titulo"),
                        DataHoraRegistro = reader.GetDateTime("dataHoraRegistro"),
                        Codigo = reader.GetString("codigo")
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar provas: {ex.Message}");
            }

            return Ok(provas);
        }

    }


}

