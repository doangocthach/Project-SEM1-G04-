using Xunit;
using System.Collections.Generic;
using BL;
using MySql.Data.MySqlClient;
using Persistence.Model;

namespace BL.Test
{
    public class OrderTest
    {
        OrderBL oBL = new OrderBL();

        [Theory]
        [InlineData(2)]
        public void Test_GetALLOrdersByIDUser(int id)
        {
            var orders = oBL.GetOrdersByCustomerId(id);
            Assert.NotNull(orders);
        }
        [Fact]
        public void DeleteOrder()
        {
            Assert.True(oBL.DeleteOrder(1));
        }
        
        [Fact]
        public void UpdateStatus()
        {
            Assert.True(oBL.UpdateStatus(1));
        }
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
            Assert.True(oBL.CreateOrder(or));
        }

    }

}