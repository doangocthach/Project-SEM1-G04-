using System;
namespace Persistence.Model
{
    public class Customer
    {

        public Customer()
        {

        }

        public Customer(int cusID, string userName, string password, string cusName, string address, string phoneNumber, decimal money)
        {
            CusID = cusID;
            UserName = userName;
            Password = password;
            CusName = cusName;
            Address = address;
            PhoneNumber = phoneNumber;
            Money = money;
        }

        public int CusID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string CusName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public decimal Money { get; set; }
    }

}