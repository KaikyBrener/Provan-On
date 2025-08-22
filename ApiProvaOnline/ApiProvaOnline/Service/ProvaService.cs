using ApiProvaOnline.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace ApiProvaOnline.Service
{
    public class ProvaService
    {
        public void CadastrarProva(Prova prova)
        {
            string sql = $@"
        INSERT INTO Prova (Titulo, DataHoraInicio, DataHoraFim, DataHoraRegistro, Codigo, IdProfessor) 
        VALUES ('{prova.Titulo}', '{prova.DataHoraInicio:yyyy-MM-dd HH:mm:ss}', '{prova.DataHoraFim:yyyy-MM-dd HH:mm:ss}', '{prova.DataHoraRegistro:yyyy-MM-dd HH:mm:ss}', '{prova.Codigo}', '{prova.IdProfessor}');
        SELECT LAST_INSERT_ID();";

            MySqlConnection connection = DbConnection.GetConnection();

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                // Executa e pega o ID gerado
                var idGerado = Convert.ToInt32(command.ExecuteScalar());

                prova.Id = idGerado;
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

        public bool RemoverProva(int id)
        {

            string sql = $"delete from prova where id = '{id}'";

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

        public Prova BuscarProva(int id)
        {
            string sql = $"select * from Prova where id = {id}";

            MySqlConnection connection = DbConnection.GetConnection();

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Prova prova = new Prova();
                    {
                        prova.Id = reader.GetInt32("id");
                        prova.Titulo = reader.GetString("Titulo");
                        prova.DataHoraInicio = reader.GetDateTime("DataHoraInicio");
                        prova.DataHoraFim = reader.GetDateTime("DataHoraFim");
                        prova.DataHoraRegistro = reader.GetDateTime("DataHoraRegistro");
                        prova.IdProfessor = reader.GetInt32("idProfessor");
                        prova.Codigo = reader.GetString("Codigo");
                    }
                    return prova;
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
        public List<Prova> ListarProva()
        {
            string sql = $"select * from Prova order by Titulo ASC";

            MySqlConnection connection = DbConnection.GetConnection();
            
            
            List<Prova> provas = new List<Prova>();

            try
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();



                while (reader.Read())
                {
                    Prova prova = new Prova();

                    prova.Id = reader.GetInt32("id");
                    prova.Titulo = reader.GetString("Titulo");
                    prova.DataHoraRegistro = reader.GetDateTime("DataHoraRegistro");
                    prova.DataHoraFim = reader.GetDateTime("DataHoraFim");
                    prova.DataHoraInicio = reader.GetDateTime("DataHoraInicio");
                    prova.Codigo = reader.GetString("Codigo");
                    prova.IdProfessor = reader.GetInt32("IdProfessor");

                    provas.Add(prova);

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
            return provas;


        }
        public bool AtualizarProva(Prova prova)
        {
            string sql = $"update prova set Titulo = '{prova.Titulo}', DataHoraInicio = '{prova.DataHoraInicio.ToString("yyyy-MM-dd HH:mm:ss")}' , DataHoraFim = '{prova.DataHoraFim.ToString("yyyy-MM-dd HH:mm:ss")}' , DataHoraRegistro = '{prova.DataHoraRegistro.ToString("yyyy-MM-dd HH:mm:ss")}', Codigo = '{prova.Codigo}', IdProfessor = '{prova.IdProfessor}' where Id = '{prova.Id}' ";

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
