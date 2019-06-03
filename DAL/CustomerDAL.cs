using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence.Model;
using System.Text.RegularExpressions;
namespace DAL
{
    public class CustomerDAL
    {
        private MySqlConnection connection;
        private MySqlDataReader reader;
        private string query;
        public CustomerDAL()
        {
            connection = DBHelper.OpenConnection();
        }

        public Customer GetCustomerByUsernameAndPassword(string userName, string password)
        {

            query = @"select * from Customers where  userName = '" + userName + "' and userPassword = '" + password + "';";

            Customer customer = null;

            reader = DBHelper.ExecQuery(query, DBHelper.OpenConnection());
            
            if (reader.Read())
            {
                customer = GetCustomer(reader);
            }

            DBHelper.CloseConnection();
            return customer;
        }
        public decimal GetMoneyByCustomerId(int? customerId)
        {
            // if (connection.State == System.Data.ConnectionState.Closed)
            // {
            //     connection.Open();
            // }
            
            query = @"select Money from Customers where CusID = " + customerId + ";";
            reader = DBHelper.ExecQuery(query,DBHelper.OpenConnection());
            Customer customer = null;
            if (reader.Read())
            {
                customer = GetMoney(reader);
            }
            DBHelper.CloseConnection();
            
            return customer.Money;
        }

        public bool UpdateMoneyCustomer(int customerId, decimal Amount)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            bool result = false;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;

            command.CommandText = "lock tables Customers write , Orders write, Items write, OrderDetail write;";
            command.ExecuteNonQuery();

            MySqlTransaction trans = connection.BeginTransaction();
            command.Transaction = trans;
            try
            {
                command.Parameters.Clear();
                command.CommandText = @"update Customers set Money = Money - @Amount where CusID = @CusID;";
                command.Parameters.AddWithValue("@Amount", Amount);
                command.Parameters.AddWithValue("@CusID", customerId);
                command.ExecuteNonQuery();
                trans.Commit();
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = false;
                trans.Rollback();
            }
            finally
            {
                command.CommandText = "unlock tables;";
                command.ExecuteNonQuery();
                DBHelper.CloseConnection();
            }

            return result;

        }
        private Customer GetCustomer(MySqlDataReader reader)
        {
            Customer customer = new Customer();
            customer.UserName = reader.GetString("userName");
            customer.Password = reader.GetString("userPassword");
            customer.Address = reader.GetString("Address");
            customer.CusID = reader.GetInt32("CusID");
            customer.PhoneNumber = reader.GetString("PhoneNumber");
            customer.CusName = reader.GetString("CusName");
            return customer;
        }

        private Customer GetMoney(MySqlDataReader reader)
        {
            Customer customer = new Customer();
            customer.Money = reader.GetDecimal("Money");
            return customer;
        }
    }
}
