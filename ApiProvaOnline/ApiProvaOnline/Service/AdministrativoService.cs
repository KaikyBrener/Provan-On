using ApiProvaOnline.Model;
using MySql.Data.MySqlClient;

namespace ApiProvaOnline.Service
{
    public class AdministrativoService
    {
        public void CadastrarAdministrativo(Administrativo administrativo)
        {
            string sql = $"insert into Administrativo (Nome, Email, Senha, DataHoraRegistro) VALUES ('{administrativo.Nome}', '{administrativo.Email}','{administrativo.Senha}', '{administrativo.DataHora.ToString("yyyy-MM-dd HH:mm:ss")}')";

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

        public bool RemoverAdministrativo(int Id)
        {
            string sql = $"delete from Administrativo where id = '{Id}'";

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

        public Administrativo BuscarAdministrativo(int Id)
        {
            string sql = $"select * from Administrativo where id = '{Id}'";

            MySqlConnection connection = DbConnection.GetConnection();

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Administrativo administrativo = new Administrativo();
                    {
                        administrativo.Id = reader.GetInt32("Id");
                        administrativo.Nome = reader.GetString("Nome");
                        administrativo.Email = reader.GetString("Email");
                        administrativo.DataHora = reader.GetDateTime("DataHoraRegistro");

                    }

                    return administrativo;

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

        public List<Administrativo> ListarAdministrativo()
        {
            string sql = $"select * from Administrativo order by Nome ASC";

            MySqlConnection connection = DbConnection.GetConnection();

            List<Administrativo> administrativos = new List<Administrativo>();

            try
            {

                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Administrativo administrativo = new Administrativo();

                    administrativo.Id = reader.GetInt32("id");
                    administrativo.Nome = reader.GetString("Nome");
                    administrativo.Email = reader.GetString("Email");
                    administrativo.DataHora = reader.GetDateTime("DataHoraRegistro");

                    administrativos.Add(administrativo);


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
            return administrativos;
        }

        public bool AtualizarAdministrativo(Administrativo administrativo)
        {
            string sql = $"update administrativo set Nome = '{administrativo.Nome}' , Email = '{administrativo.Email}' , Senha = '{administrativo.Senha}' , DataHora = '{administrativo.DataHora.ToString("yyyy-MM-dd HH:mm:ss")}' where Id = {administrativo.Id}";
            
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

            catch
            {
                throw;
            }

            finally
            {
                connection.Close();
            }
        }

        internal void CadastrarAlunoRespostaObjetiva(AlunoRespostaObjetiva alunoRespostaObjetiva)
        {
            throw new NotImplementedException();
        }
    }
}
