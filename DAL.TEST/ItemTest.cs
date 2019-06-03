using Xunit;
using System;
using Persistence.Model;
using DAL;
namespace DAL.TEST
{
    public class ItemTest
    {
        ItemDAL item = new ItemDAL();
        [Fact]
        public void GetItems()
        {
            Assert.NotNull(item.GetItems());
        }
        
        

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetItemByID(int id)
        {
            Assert.NotNull(item.GetItemByID(id));
        }
        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        public void GetItemByID2(int id)
        {
            Assert.Null(item.GetItemByID(id));
        }

        



    }


}