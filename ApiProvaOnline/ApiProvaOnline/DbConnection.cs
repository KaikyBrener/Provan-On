using MySql.Data.MySqlClient;

namespace ApiProvaOnline
{
    public static class DbConnection
    {

        // A string de conexão vem de variável de ambiente (nunca do código-fonte).
        // Ex.: ConnectionStrings__Default="Server=...;Port=3306;Database=ProvaOn;Uid=...;Pwd=..."
        public static string connectionstring =>
            Environment.GetEnvironmentVariable("ConnectionStrings__Default")
            ?? throw new InvalidOperationException(
                "Variável de ambiente ConnectionStrings__Default não definida. Configure a string de conexão do MySQL.");


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
