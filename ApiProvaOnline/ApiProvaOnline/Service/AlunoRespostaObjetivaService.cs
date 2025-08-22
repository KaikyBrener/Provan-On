using System.Security.Cryptography.X509Certificates;
using ApiProvaOnline.Model;
using MySql.Data.MySqlClient;

namespace ApiProvaOnline.Service
{
    public class AlunoRespostaObjetivaService
    {

        public void CadastrarAlunoRespostaObjetiva(AlunoRespostaObjetiva AlunoRespostaObjetiva)
        {
            string sql = $"insert into alunorespostaobjetiva (idAluno , IdPerguntaObjetiva, Resposta, PontuacaoFinal, DataHoraRegistro) VALUES ('{AlunoRespostaObjetiva.IdAluno}' , '{AlunoRespostaObjetiva.IdPerguntaObjetiva}','{AlunoRespostaObjetiva.Resposta}','{AlunoRespostaObjetiva.PontuacaoFinal}', '{AlunoRespostaObjetiva.DataHorRegistro.ToString("yyyy - MM - dd HH: mm:ss")}')";
        
            MySqlConnection connection = DbConnection.GetConnection();

            connection.Open();

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                command.ExecuteNonQuery();
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
        public bool RemoverAlunoRespostaObjetiva(int id)
        {
            string sql = $"delete * from alunorespostaobjetiva where id = {id}";

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
        public AlunoRespostaObjetiva BuscarAlunoRespostaObjetiva(int id)
        {
            string sql = $"select * from AlunoRespostaObjetiva where id = {id}";
            
            MySqlConnection connection = DbConnection.GetConnection();

            try
            {
                
                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    AlunoRespostaObjetiva alunoRespostaObjetiva = new AlunoRespostaObjetiva();
                    {
                        alunoRespostaObjetiva.Id = reader.GetInt32("id");
                        alunoRespostaObjetiva.IdPerguntaObjetiva = reader.GetInt32("idPerguntaObjetiva");
                        alunoRespostaObjetiva.DataHorRegistro = reader.GetDateTime("DataHoraRegistro");
                        alunoRespostaObjetiva.PontuacaoFinal = reader.GetChar("PontuacaoFinal");
                        alunoRespostaObjetiva.IdAluno = reader.GetInt32("IdAluno");
                        alunoRespostaObjetiva.Resposta = reader.GetChar("Resposta");

                        return alunoRespostaObjetiva;

                    }

                
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

        public bool AtualizarAlunoRespostaObjetiva(AlunoRespostaObjetiva alunoRespostaObjetiva)
        {
            string sql = $"update AlunoRespostaObjetiva set IdAluno = '{alunoRespostaObjetiva.IdAluno}' , IdPerguntaObjetiva = '{alunoRespostaObjetiva.IdPerguntaObjetiva}', Resposta = '{alunoRespostaObjetiva.Resposta}' , PontuacaoFinal = '{alunoRespostaObjetiva.PontuacaoFinal}', DataHoraRegistro = '{alunoRespostaObjetiva.DataHorRegistro.ToString("yyyy - MM - dd HH: mm:ss")}' ";
        
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
