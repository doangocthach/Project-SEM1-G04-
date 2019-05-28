using System;
using System.Collections.Generic;
using Persistence.Model;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class ItemDAL
    {
        private MySqlConnection connection;
        private string query;
        private MySqlDataReader reader;
        public ItemDAL()
        {
            connection = DBHelper.OpenConnection();
        }
        public Items GetItemByID(int? ITemID)
        {
            query = $"select * from Items where ItemID = " + ITemID + ";";
            reader = DBHelper.ExecQuery(query);
            Items item = null;
            if (reader.Read())
            {
                item = GetItem(reader);
                Console.WriteLine(item.ItemName);
            }
            DBHelper.CloseConnection();
            return item;

        }
        public List<Items> GetItems()
        {
            query = $"select * from Items;";
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(query, connection);
            List<Items> items = null;
            reader = command.ExecuteReader();
            if(reader != null)
            {
                items = GetItemsInfo(reader);
            }
            
            DBHelper.CloseConnection();
            return items;
        }
        private List<Items> GetItemsInfo(MySqlDataReader reader)
        {
            List<Items> listItems = new List<Items>();
            while (reader.Read())
            {
                Items item = new Items();
                item = GetItem(reader);
                listItems.Add(item);
            }
            return listItems;
        }

        private Items GetItem(MySqlDataReader reader)
        {
            Items it = new Items();
            it.ItemID = reader.GetInt32("ItemID");
            it.ItemName = reader.GetString("ItemName");
            it.ItemPrice = reader.GetDecimal("ItemPrice");
            it.ItemDescription = reader.GetString("ItemDescription");
            it.Size = reader.GetString("Size");

            // Console.WriteLine(item.ItemName);

            return it;
        }
    }
}