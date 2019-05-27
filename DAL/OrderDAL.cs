using System;
using Persistence.Model;
using MySql.Data.MySqlClient;
using System.Transactions;
using System.Collections.Generic;

namespace DAL
{
    public class OrderDAL
    {
        private MySqlDataReader reader;

        private MySqlConnection connection;
        private string query;

        public OrderDAL()
        {
            connection = DBHelper.OpenConnection();
        }

        public bool CreateOrder(Orders order)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            bool result = false;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;

            command.CommandText = @"lock tables Items write, Orders write,OrderDetail write;";
            command.ExecuteNonQuery();

            MySqlTransaction transactions = connection.BeginTransaction();
            command.Transaction = transactions;
            try
            {
                int? orderId = 0;

                command.CommandText = @"insert into Orders(OrderDate,Note,OrderStatus,CusID) values (@OrderDate,@Note,@OrderStatus,@CusID);";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@CusID", order.Customer.CusID);
                command.Parameters.AddWithValue("@OrderStatus", order.Status);
                command.Parameters.AddWithValue("@Note", order.Note);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.ExecuteNonQuery();
                command.CommandText = "select LAST_INSERT_ID() as OrderID";
                using (reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        orderId = reader.GetInt32("OrderID");
                    }
                }
                order.OrderID = orderId;
                foreach (var item in order.Items)
                {
                    command.Parameters.Clear();
                    command.CommandText = @"insert into OrderDetail(OrderID,ItemID) values(" + order.OrderID + ", " + item.ItemID + ");";
                    //+ "," + item.ItemCount 
                    command.ExecuteNonQuery();
                }
                transactions.Commit();
                result = true;

            }
            catch (System.Exception)
            {
                transactions.Rollback();
                throw;
            }
            finally
            {
                command.CommandText = "unlock tables";
                command.ExecuteNonQuery();
                DBHelper.CloseConnection();
            }
            return result;
        }
        public List<Orders> GetOrdersByCustomerId(int customerId)
        {

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            query = @"select * from Orders where CusID = " + customerId + ";";

            reader = DBHelper.ExecQuery(query);
            List<Orders> orders = null;
            if (reader != null)
            {
                orders = GetListOrdersInfo(reader);
            }
            DBHelper.CloseConnection();
            return orders;
        }

        public bool DeleteOrder(int? orderId)
        {
            bool result = false;


            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "lock tables Customers write , Orders write, Items write, OrderDetail write;";
            command.ExecuteNonQuery();
            MySqlTransaction trans = connection.BeginTransaction();
            command.Transaction = trans;
            try
            {
                command.Parameters.Clear();
                command.CommandText = @"delete from orderdetail where  orderId = @OrderId;";
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.CommandText = @"delete from orders where orderId = @OrderId;";
                command.Parameters.AddWithValue("@OrderId", orderId);
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
                connection.Close();
            }
            DBHelper.CloseConnection();
            return result;
        }
        private List<Orders> GetListOrdersInfo(MySqlDataReader reader)
        {
            List<Orders> listOrders = new List<Orders>();
            while (reader.Read())
            {

                Orders od = GetOrder(reader);
                listOrders.Add(od);

            }
            return listOrders;
        }

        private Orders GetOrder(MySqlDataReader reader)
        {
            // Customer customer = new Customer();
            Orders order = new Orders();
            order.OrderID = reader.GetInt32("OrderID");
            order.OrderDate = reader.GetDateTime("OrderDate");
            order.Note = reader.GetString("Note");
            order.Status = reader.GetString("OrderStatus");
            order.Customer = new Customer();
            order.Customer.CusID = reader.GetInt32("CusID");
            return order;
        }
        public bool UpdateStatusOrder(int? orderId)
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
                command.CommandText = @"Update Orders set OrderStatus = 'Complete' where OrderID = @OrderId;";
                command.Parameters.AddWithValue("@OrderId", orderId);
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
                connection.Close();
            }
            DBHelper.CloseConnection();
            return result;
        }
        private Orders GetStatus(MySqlDataReader reader)
        {
            Orders order = new Orders();
            order.Status = reader.GetString("OrderStatus");
            return order;
        }
    }
}