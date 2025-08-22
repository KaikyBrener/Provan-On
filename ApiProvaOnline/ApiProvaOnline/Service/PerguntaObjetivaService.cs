using ApiProvaOnline.Model;
using MySql.Data.MySqlClient;

namespace ApiProvaOnline.Service
{
    public class PerguntaObjetivaService
    {
        public void CadastrarPerguntaObjetiva(PerguntaObjetiva perguntaObjetiva)
        {
            string sql = $"insert into perguntaobjetiva (Titulo, Descricao, OpcaoA, OpcaoB, OpcaoC, OpcaoD, OpcaoCorreta, Pontuacao, DataHoraRegistro, Idprova) VALUES ('{perguntaObjetiva.Titulo}' , '{perguntaObjetiva.Descricao}','{perguntaObjetiva.OpcaoA}','{perguntaObjetiva.OpcaoB}', '{perguntaObjetiva.OpcaoC}','{perguntaObjetiva.OpcaoD}','{perguntaObjetiva.OpcaoCorreta}', '{perguntaObjetiva.Pontuacao}' , '{perguntaObjetiva.DataHoraRegistro.ToString("yyyy-MM-dd HH:mm:ss")}', '{perguntaObjetiva.IdProva}')";

         
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
            finally
            { 
            
                connection.Close();
            }
        
        }

        public bool RemoverPerguntaObjetiva(int Id)
        {

            string sql = $"delete from PerguntaObjetiva where id = {Id}";

            MySqlConnection connection = DbConnection.GetConnection();

            try
            {

                MySqlCommand command = new MySqlCommand( sql, connection);

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
        public PerguntaObjetiva BuscarPerguntaObjetiva(int id)
        {
            string sql = $"select * from perguntaobjetiva where id = {id}";
            
            MySqlConnection connection = DbConnection.GetConnection();

            try
            {

                MySqlCommand command = new MySqlCommand( sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                PerguntaObjetiva perguntaObjetiva = new PerguntaObjetiva();
                
                while (reader.Read())
                { 
                    perguntaObjetiva.Id = reader.GetInt32("id");
                    perguntaObjetiva.Titulo = reader.GetString("Titulo");
                    perguntaObjetiva.Descricao = reader.GetString("Descricao");
                    perguntaObjetiva.OpcaoA = reader.GetString("OpcaoA");
                    perguntaObjetiva.OpcaoB = reader.GetString("OpcaoB");
                    perguntaObjetiva.OpcaoC = reader.GetString("OpcaoC");
                    perguntaObjetiva.OpcaoD = reader.GetString("OpcaoD");
                    perguntaObjetiva.OpcaoCorreta = reader.GetChar("OpcaoCorreta");
                    perguntaObjetiva.Pontuacao = reader.GetInt32("Pontuacao");
                    perguntaObjetiva.DataHoraRegistro = reader.GetDateTime("DataHoraRegistro");
                    perguntaObjetiva.IdProva = reader.GetInt32("Idprova");
                
                    return perguntaObjetiva;

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

        public bool AtualizarPerguntaObjetiva(PerguntaObjetiva perguntaObjetiva)
        {

            string sql = $"update perguntaobjetiva set titulo = '{perguntaObjetiva.Titulo}' , descricao = '{perguntaObjetiva.Descricao}', OpcaoA = '{perguntaObjetiva.OpcaoA}' , opcaoB = '{perguntaObjetiva.OpcaoB}' , OpcaoC = '{perguntaObjetiva.OpcaoC}', OpcaoD = '{perguntaObjetiva.OpcaoD}', OpcaoCorreta = '{perguntaObjetiva.OpcaoCorreta}' , DataHoraRegistro = '{perguntaObjetiva.DataHoraRegistro.ToString("yyyy-MM-dd HH:mm:ss")}' , Pontuacao = '{perguntaObjetiva.Pontuacao}', IdProva = '{perguntaObjetiva.IdProva}' where id = '{perguntaObjetiva.Id}'";

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
