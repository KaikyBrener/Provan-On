using MySql.Data.MySqlClient;

namespace ApiProvaOnline
{
    public static class DbConnection
    {

        public static string connectionstring = "Server=turntable.proxy.rlwy.net;Port=31724;Database=SistemaDeProvas;Uid=root;Pwd=zkiHnKGGowUqZRrzzdLVxIwxsVqdVKlE";


        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionstring);

                connection.Open();

                return connection;

            }
            catch (Exception)
            {

                throw;
            }
        
        }

        public static void CloseConnection(MySqlConnection connection)
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            
        }

    }
}
