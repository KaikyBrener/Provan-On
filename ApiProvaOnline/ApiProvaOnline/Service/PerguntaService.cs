using System.Security.Cryptography.X509Certificates;
using ApiProvaOnline.Model;
using MySql.Data.MySqlClient;

namespace ApiProvaOnline.Service
{
    public class PerguntaService
    {

        public void CadastrarPergunta(Pergunta pergunta)
        {

            string sql = $"insert into pergunta (titulo, descricao, pontuacao,datahoraregistro, idprova) VALUES ('{pergunta.Titulo}', '{pergunta.Descricao}', '{pergunta.Pontuacao}' , '{pergunta.DataHoraRegistro.ToString("yyyy-MM-dd HH:mm:ss")}' , '{pergunta.IdProva}')";

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
        public bool RemoverPergunta(int id)
        {
            string sql = $"delete from pergunta where id = '{id}'";

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
        public Pergunta BuscarPergunta(int Id)
        {
            string sql = $"select * from Pergunta where id = '{Id}'";

            MySqlConnection connection = DbConnection.GetConnection();

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                Pergunta pergunta = new Pergunta();

                while (reader.Read())
                {
                    pergunta.Id = reader.GetInt32("Id");
                    pergunta.Titulo = reader.GetString("Titulo");
                    pergunta.Descricao = reader.GetString("Descricao");
                    pergunta.Pontuacao = reader.GetDouble("Pontuacao");
                    pergunta.DataHoraRegistro = reader.GetDateTime("DataHoraRegistro");
                    pergunta.IdProva = reader.GetInt32("Idprova");

                }
                return pergunta;
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

        public bool AtualizarPergunta(Pergunta pergunta)
        { 
            string sql = $"update pergunta set Titulo = '{pergunta.Titulo}', Descricao = '{pergunta.Descricao}' , Idprova = '{pergunta.IdProva}' , pontuacao = '{pergunta.Pontuacao}' , datahoraregistro = '{pergunta.DataHoraRegistro.ToString("yyyy-MM-dd HH:mm:ss")}' where Id = '{pergunta.Id}'";

            MySqlConnection connetion = DbConnection.GetConnection();

            try
            {

                MySqlCommand command = new MySqlCommand(sql , connetion);

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
                connetion.Close();
            }


        }
        public List<Pergunta> ListarPergunta()
        {
            string sql = "select * from Pergunta order by DataHoraRegistro";

            MySqlConnection connection = DbConnection.GetConnection();

            List<Pergunta> perguntas = new List<Pergunta>();

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Pergunta pergunta = new Pergunta();

                    pergunta.Id = reader.GetInt32("id");
                    pergunta.Titulo = reader.GetString("Titulo");
                    pergunta.Descricao = reader.GetString("Descricao");
                    pergunta.DataHoraRegistro = reader.GetDateTime("DataHoraRegistro");
                    pergunta.IdProva = reader.GetInt32("IdProva");
                    pergunta.Pontuacao = reader.GetInt32("Pontuacao");

                    perguntas.Add(pergunta);

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
            return perguntas;

        }

    }

}
