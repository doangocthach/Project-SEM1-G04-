using System;
using Xunit;
using BL;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence.Model;
namespace BL.Test
{
    public class UnitTest1
    {
        ItemBL iBl = new ItemBL();
        [Fact]
        public void GetItemsTest()
        {
            List<Items> items = iBl.GetItems();
            Assert.NotNull(items);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void GetItemByIdTest(int Id)
        {
            Items item = iBl.GetItemByID(Id);
            Assert.NotNull(item);
        }


        [Fact]
        public void GetItemByIdTest2()
        {
            Items item = iBl.GetItemByID(1000);
            Assert.Null(item);
        }
    }
}
