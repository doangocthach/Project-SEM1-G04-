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
    }


}