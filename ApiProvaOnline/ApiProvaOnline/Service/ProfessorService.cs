using System.Data;
using System.Security.Cryptography.X509Certificates;
using ApiProvaOnline.Model;
using MySql.Data.MySqlClient;

namespace ApiProvaOnline.Service
{
    public class ProfessorService
    {
        public void CadastrarProfessor(Professor professor)
        {
            string sql = $"insert into Professor (Nome, Email, Senha, DataHoraRegistro) VALUES ('{professor.Nome}', '{professor.Email}','{professor.Senha}', '{professor.DataHoraRegistro.ToString("yyyy-MM-dd HH:mm:ss")}')";



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
        public bool RemoverProfessor(int Id)
        {
            string sql = $"delete from Professor where Id = '{Id}'";

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
        public Professor BuscarProfessor(int Id)
        {

            string sql = $"select * from Professor where Id = '{Id}'";

            MySqlConnection connection = DbConnection.GetConnection();

            try
            {

                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Professor professor = new Professor();

                    {
                        professor.Id = reader.GetInt32("id");
                        professor.Nome = reader.GetString("Nome");
                        professor.Email = reader.GetString("Email");
                        professor.DataHoraRegistro = reader.GetDateTime("DataHoraRegistro");


                    }

                    return professor;

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

        public List<Professor> ListarProfessores()
        {
            string sql = "select * from Professor order by Nome ASC";

            MySqlConnection connection = DbConnection.GetConnection();

            List<Professor> professores = new List<Professor>();

            try
            {

                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Professor professor = new Professor();

                    professor.Id = reader.GetInt32("id");
                    professor.Nome = reader.GetString("Nome");
                    professor.Email = reader.GetString("Email");
                    professor.DataHoraRegistro = reader.GetDateTime("DataHoraRegistro");

                    professores.Add(professor);


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
            return professores;



        }

        public bool AtualizarProfessor(Professor professor)
        {

            string sql = $"update Professor set Nome = '{professor.Nome}' , Email = '{professor.Email}' , Senha = '{professor.Senha}' , DataHoraRegistro = '{professor.DataHoraRegistro.ToString("yyyy-MM-dd HH:mm:ss")}'  where Id = {professor.Id}";

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


        public Professor LoginProfessor(string email, string senha)
        {
            string sql = $"SELECT * FROM Professor WHERE Email = '{email}' AND Senha = '{senha}'";

            MySqlConnection connection = DbConnection.GetConnection();

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Professor professor = new Professor
                    {
                        Id = reader.GetInt32("Id"),
                        Nome = reader.GetString("Nome"),
                        Email = reader.GetString("Email"),
                        Senha = reader.GetString("Senha"), // você pode optar por não retornar a senha na resposta da API
                        DataHoraRegistro = reader.GetDateTime("DataHoraRegistro")
                    };

                    return professor;
                }

                return null;
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
