using System;
using Xunit;
using BL;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence.Model;

namespace BL.Test
{
    public class CustomerTest
    {
        CustomerBL CustomerBL = new CustomerBL();
        [Theory]
        [InlineData("Customer_1", "dung1234567800")]
        [InlineData("Customer_2", "A12345678")]
        public void GetCustomerByUsernamrAndPasswordNull(string username, string password)
        {
            Customer cu = CustomerBL.GetCustomerByUsernameAndPassword(username, password);

            Assert.Null(cu);
        }
        [Theory]
        [InlineData("Thach", "1")]
        [InlineData("Toan123", "1")]
        public void GetCustomerByUsernamrAndPasswordNotNull(string username, string password)
        {
            Customer cu = CustomerBL.GetCustomerByUsernameAndPassword(username, password);

            Assert.NotNull(cu);
        }
        

    }
}