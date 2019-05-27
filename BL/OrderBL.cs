using System;
using System.Collections.Generic;
using DAL;
using Persistence.Model;

namespace BL
{

    public class OrderBL
    {
        public bool CreateOrder(Orders order)
        {
            OrderDAL orderDAL = new OrderDAL();
            return orderDAL.CreateOrder(order);
        }
        public List<Orders> GetOrdersByCustomerId(int customerId)
        {
            OrderDAL orderDAL = new OrderDAL();
        
            List<Orders> listOrders = orderDAL.GetOrdersByCustomerId(customerId);

            return listOrders;
        }
        public bool DeleteOrder(int? orderId)
        {
            OrderDAL orderDAL = new OrderDAL();
            return orderDAL.DeleteOrder(orderId);
        }
        public bool UpdateStatus(int? orderId)
        {
            OrderDAL orderDAL = new OrderDAL();
            return orderDAL.UpdateStatusOrder(orderId);
        }
    }
}