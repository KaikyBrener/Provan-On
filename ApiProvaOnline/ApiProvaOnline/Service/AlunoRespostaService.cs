using System.Security.Cryptography.X509Certificates;
using ApiProvaOnline.Model;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MySql.Data.MySqlClient;

namespace ApiProvaOnline.Service
{
    public class AlunoRespostaService
    {

        public void CadastrarAlunoResposta(AlunoResposta alunoResposta)
        {
            string sql = $"insert into AlunoResposta (IdAluno, IdPergunta , Resposta, PontuacaoFinal , DataHoraRegistro) Values ('{alunoResposta.IdAluno}', '{alunoResposta.IdPergunta}','{alunoResposta.Resposta}', '{alunoResposta.PontuacaoFinal}','{alunoResposta.DataHoraRegistro.ToString("yyyy - MM - dd HH: mm:ss")}')";
        
            MySqlConnection connection = DbConnection.GetConnection();

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                command.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally { 
            
                connection.Close();
            
            }


        }

        public bool RemoverAlunoResposta(int Id)
        {

            string sql = $"delete * from AlunoResposta where id = {Id}";

            MySqlConnection connection = DbConnection.GetConnection();

            try
            {

                MySqlCommand command = new MySqlCommand(sql , connection);

                int LinhasAfetadas = command.ExecuteNonQuery();

                if (LinhasAfetadas > 0)
                {

                    return true;

                }
                else
                {

                    return false;

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            { 
            
                connection.Close();
            
            }
        
        
        }

        public AlunoResposta BuscarAlunoResposta(int id)
        {
            string sql = $"select * from AlunoResposta where id = {id}";

            MySqlConnection connection = DbConnection.GetConnection();

            try
            {

                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    AlunoResposta alunoResposta = new AlunoResposta();

                    {
                        alunoResposta.Id = reader.GetInt32("id");
                        alunoResposta.IdAluno = reader.GetInt32("IdAluno");
                        alunoResposta.IdPergunta = reader.GetInt32("IdPergunta");
                        alunoResposta.Resposta = reader.GetString("Resposta");
                        alunoResposta.PontuacaoFinal = reader.GetString("PontuacaoFinal");
                        alunoResposta.DataHoraRegistro = reader.GetDateTime("DataHoraRegistro");

                    }

                    return alunoResposta;

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

                connection.Close();
            
            }
            return null;

        }

        public bool AtualizarAlunoResposta(AlunoResposta alunoResposta)
        {

            string sql = $"update alunoresposta set IdAluno = '{alunoResposta.IdAluno}' , IdPergunta = '{alunoResposta.IdPergunta}' , Resposta = '{alunoResposta.Resposta}' , PontuacaoFinal = '{alunoResposta.PontuacaoFinal}' , DataHoraRegistro  = '{alunoResposta.DataHoraRegistro.ToString("yyyy - MM - dd HH: mm:ss")}'";
            
            MySqlConnection connection = DbConnection.GetConnection();

            try
            {

                MySqlCommand command = new MySqlCommand(sql, connection);

                int LinhasAfetadas = command.ExecuteNonQuery();

                if (LinhasAfetadas > 0)
                {

                    return true;

                }
                else
                {

                    return false;

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            { 
            
                connection.Close();

            }
        
        }


    }
}
