using DAL;
using Persistence.Model;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace BL
{
    public class ItemBL
    {

        public Items GetItemByID(int? itemID)
        {
            ItemDAL itDal = new ItemDAL();
            return itDal.GetItemByID(itemID);
        }

        public List<Items> GetItems()
        {
            ItemDAL itDal = new ItemDAL();
            return itDal.GetItems();
        }

    }
}