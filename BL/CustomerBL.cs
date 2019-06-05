using System;
using System.Text.RegularExpressions;
using Persistence.Model;
using DAL;
namespace BL
{
    public class CustomerBL
    {
        private CustomerDAL cusDAL = new CustomerDAL();
        public Customer GetCustomerByUsernameAndPassword(string userName, string password)
        {
            // Console.WriteLine("ABC");
            return cusDAL.GetCustomerByUsernameAndPassword(userName, password);
        }

  
        public bool UpdateMoneyCustomer(int customerId, decimal money)
        {
            return cusDAL.UpdateMoneyCustomer(customerId, money);
        }

    }
}
