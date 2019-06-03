using System;
using Xunit;
using Persistence;
using DAL;

namespace DAL.TEST
{
    public class DbHelperTest
    { 
        // DBHelper dbHelper = new DBHelper();
        [Fact]
        public void OpenConnectionTest()
        {
            Assert.NotNull(DBHelper.OpenConnection());
        }
        
    }
}