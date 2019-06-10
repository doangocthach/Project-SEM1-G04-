using System;
using Xunit;
using Persistence.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace DAL.TEST
{
    public class OrderTest
    {

        OrderDAL oDAl = new OrderDAL();
        [Fact]
        public void Create_test()
        {
            Orders or = new Orders();
            or.Customer = new Customer();
            or.OrderID = 1;
            or.Status = "complete";
            or.Customer.CusID = 1;
            Items items = new Items();
            or.Items = new List<Items>();
            items.ItemID = 1;
            items.ItemPrice = 2;
            or.Items.Add(items);
            Assert.True(oDAl.CreateOrder(or));
        }
        [Fact]
        public void DeleteOrder()
        {
           Assert.True(oDAl.DeleteOrder(1));
        }
        [Fact]
        public void UpdateStatus()
        {
          Assert.True(oDAl.UpdateStatusOrder(1));
        }
        [Theory]
        [InlineData(2)]
        public void GetOrdersByCustomerIdTest(int customerId)
        {
            var TEST = oDAl.GetOrdersByCustomerId(customerId);
            Assert.NotNull(TEST);
        }
    }
}
