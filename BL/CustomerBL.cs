using System;
using System.Text.RegularExpressions;
using Persistence.Model;
using DAL;
namespace BL
{
    public class CustomerBL
    {
        CustomerDAL cusDAL = new CustomerDAL();

        public Customer GetCustomerByUsernameAndPassword(string userName, string password)
        {
            return cusDAL.GetCustomerByUsernameAndPassword(userName, password);
        }

        public decimal GetMoneybyCustomerId(int? customerId)
        {
            return cusDAL.GetMoneyByCustomerId(customerId);
        }
        public bool UpdateMoneyCustomer(int customerId, decimal money)
        {
            return cusDAL.UpdateMoneyCustomer(customerId, money);
        }

    }
}
