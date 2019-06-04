using System;
using System.Collections.Generic;
namespace Persistence.Model
{
    public class Orders
    {


        public Orders()
        {

        }

        public Orders(int? orderID, int itemID, DateTime orderDate, string note, string status, Customer customer, decimal amount, List<Items> items, Items item)
        {
            OrderID = orderID;
            ItemID = itemID;
            OrderDate = orderDate;
            Note = note;
            Status = status;
            Customer = customer;
            Amount = amount;
            Items = items;
            Item = item;
        }

        public int? OrderID { get; set; }
        public int ItemID { get; set; }
        public DateTime OrderDate { get; set; }

        public string Note { get; set; }

        public string Status { get; set; }

        public Customer Customer { get; set; }

        public decimal Amount { get; set; }
        public List<Items> Items { get; set; }

        public Items Item {get;set;}
        

        public override bool Equals(object obj)
        {
            return obj is Orders orders &&
                   OrderID == orders.OrderID;
        }

        public override int GetHashCode()
        {
            return (OrderID + Status + Note + Customer + OrderDate + Items).GetHashCode();
        }


    }
}