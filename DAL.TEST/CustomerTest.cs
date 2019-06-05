using Xunit;
using System;
using Persistence.Model;
using DAL;

namespace DAL.TEST
{
    public class CustomerTest
    {
        private CustomerDAL customerDAL = new CustomerDAL();

        [Theory]
        [InlineData("Thach", "1")]
        [InlineData("Toan123", "1")]
        public void GetCustomerByUsernamAndPasswordTest1(string username, string password)
        {
            Customer customer = customerDAL.GetCustomerByUsernameAndPassword(username, password);
            Assert.NotNull(customer);
            Assert.Equal(username, customer.UserName);
            Assert.Equal(password, customer.Password);
        }
        [Theory]
        [InlineData("customer_01", "12345688889")]
        [InlineData("customer_02", "1212688889")]
        [InlineData("@#!@!@#!@#", "!@#!@#!@#$")]
        [InlineData("customer_06", "1212688889")]
        public void GetCustomerByUsernamAndPasswordTest2(string username, string password)
        {
            Customer cs = customerDAL.GetCustomerByUsernameAndPassword(username, password);
            Assert.Null(cs);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void UpdateMoneyTest(int customerId, decimal amount)
        {
            Assert.True(customerDAL.UpdateMoneyCustomer(customerId, amount));
        }


    }
}