using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class DBHelper
    {
        private static MySqlConnection connection;
        public static MySqlConnection GetConnection()
        {
            string connectionString;
            try
            {
                FileStream fileStream = File.OpenRead("ConnectionString.txt");
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    connectionString = reader.ReadLine();
                }
                fileStream.Close();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Error File connectionString.txt");
                return null;
            }
            try
            {
                connection = new MySqlConnection { ConnectionString = connectionString };
                return connection;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
        public static MySqlConnection OpenConnection()
        {
            if (connection == null)
            {
                GetConnection();
            }
            connection.Open();
            return connection;
        }
        public static void CloseConnection()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
        public static MySqlDataReader ExecQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteReader();
        }
    }
}
