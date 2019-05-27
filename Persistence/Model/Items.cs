using System;

namespace Persistence.Model
{
    public class Items
    {

        public Items()
        {
            
        }

        public Items(int itemCount, decimal itemPrice, int itemID, string itemName, int count, string itemDescription, string size)
        {
            ItemCount = itemCount;
            ItemPrice = itemPrice;
            ItemID = itemID;
            ItemName = itemName;
            Count = count;
            ItemDescription = itemDescription;
            Size = size;
        }

        public int ItemCount{get;set;}
        public decimal  ItemPrice{get;set;}

        public int ItemID {get;set;}

        public string ItemName{get;set;}

        public int Count;

        public string ItemDescription{get;set;}

        public string Size {get;set;}
    }
    
}