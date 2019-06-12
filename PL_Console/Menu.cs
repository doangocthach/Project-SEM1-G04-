using System;
using System.Text;
using System.Text.RegularExpressions;
using BL;
using Persistence.Model;
// using System.Security;
using System.Collections.Generic;
using ConsoleTables;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

namespace PL_Console
{
    public class Menu
    {
        string row = ("==================================================================");
        decimal amount = 0;

        private Customer customer = new Customer();
        List<Items> items = new List<Items>();
        public void MainMenu()
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=================== WELCOME TO VTCA CAFFE !=======================");
                string choice;
                Console.WriteLine("1. Login ");
                Console.WriteLine("2. Exit ");
                Console.Write("Enter your selection : ");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        LoginMenu();
                        break;
                    case "2":
                        Console.WriteLine("See you again ! ");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("input invalid !");
                        continue;
                }
            }
        }
        public string checkYN()
        {
            string choice = Console.ReadLine().ToUpper();
            while (true)
            {
                if (choice != "Y" && choice != "N")
                {
                    Console.Write("You can only enter (Y/N): ");
                    choice = Console.ReadLine().ToUpper();
                    continue;
                }
                break;
            }
            switch (choice)
            {
                case "Y":
                    break;
                case "N":
                    break;

            }
            return choice;
        }
        public void Pay(Orders or)
        {
            CustomerBL cuBL = new CustomerBL();
            OrderBL ordersbl = new OrderBL();
            or.Customer = new Customer();

            while (true)
            {
                Console.Clear();
                Console.WriteLine(row);
                Console.WriteLine("PAY");
                Console.WriteLine(row);



                if (customer.Money < amount)
                {
                    Console.WriteLine("Your card does not have enough money ! ");
                    Console.ReadKey();
                    MenuAfterLogin();
                }
                else
                {
                    try
                    {
                        bool UpdateMoney = cuBL.UpdateMoneyCustomer(customer.CusID, amount);

                        bool UpdateStatus = ordersbl.UpdateStatus(or.OrderID);
                        customer.Money = customer.Money - amount;
                    }
                    catch (System.Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                        Console.ReadKey();
                        break;
                    }

                    // Console.WriteLine("\nPay success !\n");
                    try
                    {
                        // Check if file exists with its full path    
                        if (File.Exists(Path.Combine("shoppingcart" + customer.UserName + ".dat")))
                        {
                            // If file found, delete it    
                            File.Delete(Path.Combine("shoppingcart" + customer.UserName + ".dat"));
                        }
                        else Console.WriteLine("Cart not found");
                    }
                    catch (IOException ioExp)
                    {
                        Console.WriteLine(ioExp.Message);
                    }


                    Console.WriteLine("\n  Pay success, press anykey to continue !\n");
                    ShowBill(or.OrderID);
                    amount = 0;
                    Console.ReadKey();
                    MenuAfterLogin();
                    break;
                }


            }
        }
        public void AddToCart(Items item)
        {
            if (File.Exists("shoppingcart" + customer.UserName + ".dat"))
            {
                FileStream fs = new FileStream("shoppingcart" + customer.UserName + ".dat", FileMode.Open, FileAccess.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                string str = br.ReadString();
                items = JsonConvert.DeserializeObject<List<Items>>(str);
                fs.Close();
                br.Close();
                items.Add(item);
            }
            else
            {
                items.Add(item);
            }

            Customer cust = new Customer();
            string sJSONResponse = JsonConvert.SerializeObject(items);
            BinaryWriter bw;
            string fileName = "shoppingcart" + customer.UserName + ".dat";

            try
            {
                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                bw = new BinaryWriter(fs);
                bw.Write((string)(object)sJSONResponse);
                fs.Close();

            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex);
            }
            Console.WriteLine("\nAdd to cart success !\n");
        }
        public void ShowCart()
        {

            while (true)
            {
                Console.Clear();
                Orders or = new Orders();
                if (File.Exists("shoppingcart" + customer.UserName + ".dat"))
                {
                    List<Items> itemsa = null;
                    try
                    {
                        FileStream fs = new FileStream("shoppingcart" + customer.UserName + ".dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        BinaryReader br = new BinaryReader(fs);
                        string a = br.ReadString();
                        itemsa = JsonConvert.DeserializeObject<List<Items>>(a);
                        br.Close();
                        fs.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.ReadKey();
                        break;
                    }
                    decimal AmountPay = 0;
                    Console.WriteLine(row);
                    Console.WriteLine("SHOPPING CART");
                    Console.WriteLine(row);

                    var table = new ConsoleTable("ID", "ITEM NAME", "QUANTITY", "PRICE", "SIZE", "TOTLE");
                    foreach (Items itema in itemsa)
                    {
                        decimal totle = itema.ItemPrice * itema.ItemCount;
                        table.AddRow(itema.ItemID, itema.ItemName, itema.ItemCount, itema.ItemPrice, itema.Size, totle);
                        amount += itema.ItemPrice * itema.ItemCount;
                        AmountPay += totle;
                    }

                    table.Write();
                    Console.WriteLine("\nAmount : {0}\n", AmountPay);
                    Console.WriteLine("1. Pay");
                    Console.WriteLine("2. Delete shopping cart");
                    Console.WriteLine("3. Delete an item");
                    Console.WriteLine("4. Back");
                    string choice;
                    Console.Write("Enter your selection : ");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            CreateOrder(or, itemsa);
                            break;
                        case "2":
                            Console.Write("Do you want delete shopping card (Y/N)");
                            string check = checkYN().ToUpper();
                            if (check == "Y")
                            {
                                try
                                {
                                    // Check if file exists with its full path    
                                    if (File.Exists(("shoppingcart" + customer.UserName + ".dat")))
                                    {
                                        // If file found, delete it    
                                        File.Delete(("shoppingcart" + customer.UserName + ".dat"));
                                        items.Clear();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cart not found");
                                    }
                                }
                                catch (IOException ioExp)
                                {
                                    Console.WriteLine(ioExp.Message);
                                }
                                break;
                            }
                            else
                            {

                            }
                            break;
                        case "3":
                            int deleteId;
                            int temp = 1;


                            while (true)
                            {

                                Console.Write("Enter the item ID you want to delete or enter 0 to back:");
                                try
                                {
                                    deleteId = int.Parse(Console.ReadLine());
                                    if (deleteId == 0)
                                    {
                                        break;
                                    }
                                    for (int i = 0; i < itemsa.Count; i++)
                                    {
                                        if (itemsa[i].ItemID == deleteId)
                                        {
                                            temp = 0;
                                            amount = 0;
                                            items.Clear();
                                            itemsa.RemoveAt(i);
                                            string fileName = "shoppingcart" + customer.UserName + ".dat";
                                            File.Delete(fileName);
                                            string sJSONResponse = JsonConvert.SerializeObject(itemsa);
                                            BinaryWriter bw;
                                            try
                                            {
                                                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                                bw = new BinaryWriter(fs);
                                                bw.Write((string)(object)sJSONResponse);
                                                fs.Close();
                                            }
                                            catch (System.Exception ex)
                                            {
                                                Console.WriteLine(ex);
                                            }
                                        }

                                    }
                                    if (itemsa.Count < 1)
                                    {
                                        try
                                        {
                                            // Check if file exists with its full path    
                                            if (File.Exists(("shoppingcart" + customer.UserName + ".dat")))
                                            {
                                                // If file found, delete it    
                                                File.Delete(("shoppingcart" + customer.UserName + ".dat"));
                                                items.Clear();
                                            }
                                            else
                                            {
                                                Console.WriteLine("Cart not found");
                                            }
                                        }
                                        catch (IOException ioExp)
                                        {
                                            Console.WriteLine(ioExp.Message);
                                        }
                                    }
                                    if (temp == 1)
                                    {
                                        Console.WriteLine("No item found , press anykey to back");
                                        amount = 0;
                                        Console.ReadKey();
                                        break;
                                    }
                                    break;
                                }
                                catch (System.Exception)
                                {
                                    Console.WriteLine("You must enter id exists!");
                                    Console.ReadKey();
                                    continue;
                                }
                            }
                            continue;
                        case "4":
                            MenuAfterLogin();
                            break;

                        default:
                            Console.WriteLine("Please enter in options !");
                            continue;
                    }

                }
                else
                {
                    Console.WriteLine("\nNo shopping cart yet , press anykey to continue !\n");
                    Console.ReadKey();
                    break;
                }
            }

        }
        public void CreateOrder(Orders or, List<Items> itemsa)
        {


            CultureInfo provider = CultureInfo.InvariantCulture; ;
            OrderBL ordersbl = new OrderBL();
            Console.Write("Enter your note: ");
            string note = Console.ReadLine();
            or.Note = note;
            // string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            or.OrderDate = DateTime.Now;
            or.Status = "Not yet";
            or.Customer = customer;
            ItemBL itBl = new ItemBL();
            or.Items = new List<Items>();
            or.Items = itemsa;
            bool a = true;
            bool b;

            while (true)
            {

                Console.Clear();
                string check;
                string choice = null;
                Console.WriteLine(row);
                Console.WriteLine("PAY");
                Console.WriteLine(row);
                Console.WriteLine("\n1. Pay by ATM card ");
                Console.WriteLine("2. Pay down ");
                Console.WriteLine("3. Cancel order");

                Console.Write("Enter your select : ");

                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        while (true)
                        {
                            Console.WriteLine("1. Confirm ");
                            Console.WriteLine("2. Back");
                            Console.Write("Enter your selection :");
                            string select = Console.ReadLine();
                            switch (select)
                            {
                                case "1":
                                    Console.WriteLine("Press anykey to pay !");
                                    Console.ReadKey();
                                    if (customer.Money < amount)
                                    {
                                        Console.WriteLine("Your card does not have enough money ! ");
                                        Console.ReadKey();
                                        break;
                                    }
                                    a = ordersbl.CreateOrder(or);
                                    items.Clear();
                                    Pay(or);
                                    break;
                                case "2":

                                    break;

                                default:
                                    Console.WriteLine("You must enter integer and in the option !");
                                    Console.ReadKey();
                                    continue;
                            }
                            break;
                        }

                        break;

                    case "2":
                        decimal cash;
                        while (true)
                        {
                            Console.WriteLine("The money you must pay : " + amount);
                            Console.Write("Enter amount must pay:");
                            bool c = Decimal.TryParse(Console.ReadLine(), out cash);
                            if (c == false)
                            {
                                Console.WriteLine("You must enter number ! press anykey to continue");
                                Console.ReadKey();
                                continue;
                            }
                            if (cash == amount)
                            {
                                a = ordersbl.CreateOrder(or);
                                items.Clear();
                                Pay(or);
                            }
                            if (cash < amount)
                            {
                                Console.Write("Amout must greater than totle price, do you continue (Y/N)");
                                check = checkYN();
                                if (check == "N")
                                {
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            if (cash > amount)
                            {
                                decimal excessCash = amount - cash;
                                Console.WriteLine("\npay back the excess {0}, press anykey to continue\n", FomatMoney(excessCash));
                                Console.ReadKey();
                                a = ordersbl.CreateOrder(or);
                                items.Clear();
                                Pay(or);
                            }

                        }
                        break;
                    case "3":
                        b = ordersbl.DeleteOrder(or.OrderID);
                        Console.WriteLine("Order has been canceled, Press anykey to continue !");
                        Console.ReadKey();
                        MenuAfterLogin();
                        break;

                    default:
                        Console.WriteLine("input invalid !");
                        break;
                }



            }

        }
        public void ShowOrderBought(int CustomerId)
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine(row);
                Console.WriteLine("BOUGHT ORDER");
                Console.WriteLine(row);
                List<Orders> orders = null;
                try
                {
                    OrderBL orderBL = new OrderBL();
                    orders = orderBL.GetOrdersByCustomerId(CustomerId);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                    continue;
                }

                if (orders.Count == 0)
                {
                    Console.WriteLine("List orders empty, press anykey to continue !\n");
                    Console.ReadKey();
                    break;
                }
                var table = new ConsoleTable("ORDER ID", "ORDER DATE", "NOTE", "ORDER STATUS");
                foreach (Orders ord in orders)
                {
                    table.AddRow(ord.OrderID, ord.OrderDate.ToString("dddd, dd MMMM yyyy"), ord.Note, ord.Status);
                }
                table.Write();

                string choice;
                Console.WriteLine("1. View detail of order");
                Console.WriteLine("2. Back.");
                Console.Write("Enter your selection: ");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        while (true)
                        {
                            Console.Write("Enter the order ID you want to see:  ");
                            try
                            {
                                int orderid = int.Parse(Console.ReadLine());

                                if (orderid > orders.Count)
                                {
                                    Console.WriteLine("You must enter order ID exists !");
                                    continue;
                                }
                                ShowBill(orderid);
                                break;
                            }
                            catch (System.Exception)
                            {

                                Console.WriteLine("You must enter order ID exists !");
                                continue;
                            }
                        }

                        break;
                    case "2":
                        MenuAfterLogin();
                        break;
                    default:
                        Console.WriteLine("You must enter integer and in the option !");
                        Console.ReadKey();
                        continue;
                }
            }
        }
        public void showItems()
        {
            while (true)
            {


                Console.Clear();
                Console.WriteLine(row);
                Console.WriteLine("MENU");
                Console.WriteLine(row);
                ItemBL itemBL = new ItemBL();
                List<Items> li = null;
                try
                {
                    li = itemBL.GetItems();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }
                var table = new ConsoleTable("ID", "ITEM'S NAME", "PRICE OF SIZE M", "PRICE OF SIZE X", "PRICE OF SIZE XL");
                foreach (Items item in li)
                {
                    table.AddRow(item.ItemID, item.ItemName, FomatMoney(item.ItemPrice), FomatMoney(item.ItemPrice + 5), FomatMoney(item.ItemPrice + 10));
                }
                table.Write();

                string selction;

                Console.WriteLine("1. View item detail.");
                Console.WriteLine("2. Search item.");
                Console.WriteLine("3. Back.");
                Console.Write("Enter your selection: ");

                selction = Console.ReadLine();
                switch (selction)
                {
                    case "1":
                        int itID;
                        string choice;
                        while (true)
                        {
                            try
                            {
                                li = itemBL.GetItems();
                                Console.Write("Enter item id: ");
                                itID = int.Parse(Console.ReadLine());

                                break;
                            }
                            catch (System.Exception)
                            {
                                Console.WriteLine("Item id must be integer and in the options !");
                                continue;
                            }
                        }
                        if (itID > li.Count || validateChoice(itID.ToString()) == false)
                        {
                            Console.Write("You are only entered in the number of existing ids !");
                            while (true)
                            {
                                Console.Write("Do  you want re-enter ? (Y/N): ");
                                choice = Console.ReadLine().ToUpper();
                                if (choice != "Y" && choice != "N")
                                {
                                    Console.Write("You can only enter  (Y/N): ");
                                    choice = Console.ReadLine().ToUpper();
                                    continue;
                                }
                                break;
                            }

                            switch (choice)
                            {
                                case "Y":
                                    continue;

                                case "N":
                                    showItems();
                                    break;

                                default:
                                    continue;
                            }
                        }
                        showItemDetail(itID);
                        break;
                    case "2":
                        string str;
                        int temp = 0;
                        Console.InputEncoding = Encoding.Unicode;

                        Console.Write("Enter the item to search: ");
                        str = Console.ReadLine().Trim();

                        var tables = new ConsoleTable("ID", "ITEM'S NAME", "PRICE OF SIZE M", "PRICE OF SIZE X", "PRICE OF SIZE XL");
                        foreach (var item in li)
                        {
                            if (item.ItemName.ToUpper().Contains(str.ToUpper()))
                            {
                                tables.AddRow(item.ItemID, item.ItemName, FomatMoney(item.ItemPrice), FomatMoney(item.ItemPrice + 5), FomatMoney(item.ItemPrice + 10));
                                temp = 1;
                            }
                        }
                        if (temp == 0)
                        {
                            Console.WriteLine("No items found, press anykey to continue !");
                            Console.ReadKey();
                            break;
                        }
                        Console.Clear();
                        Console.WriteLine("RESULT FOR: {0}\n", str);
                        tables.Write();
                        int itemid;
                        string choicee;
                        while (true)
                        {
                            try
                            {
                                li = itemBL.GetItems();
                                Console.Write("Enter item id: ");
                                itemid = int.Parse(Console.ReadLine());

                                break;
                            }
                            catch (System.Exception)
                            {
                                Console.WriteLine("Item id must be integer and in the options !");
                                continue;
                            }
                        }
                        if (itemid > li.Count || validateChoice(itemid.ToString()) == false)
                        {
                            Console.Write("You are only entered in the number of existing ids !");
                            while (true)
                            {
                                Console.Write("Do  you want re-enter ? (Y/N): ");
                                choicee = Console.ReadLine().ToUpper();
                                if (choicee != "Y" && choicee != "N")
                                {
                                    Console.Write("You can only enter  (Y/N): ");
                                    choice = Console.ReadLine().ToUpper();
                                    continue;
                                }
                                break;
                            }

                            switch (choicee)
                            {
                                case "Y":
                                    continue;

                                case "N":
                                    showItems();
                                    break;

                                default:
                                    continue;
                            }
                        }
                        showItemDetail(itemid);
                        Console.ReadKey();
                        break;
                    case "3":
                        MenuAfterLogin();
                        break;
                    default:
                        Console.WriteLine("You must enter integer and in the option !");
                        Console.ReadKey();
                        break;
                }

            }

        }
        public void showItemDetail(int itID)
        {

            while (true)
            {
                Console.Clear();
                string row = ("==================================================================");
                ItemBL itBL = new ItemBL();
                Items it = new Items();
                // List<Items> li = null;
                Customer custom = new Customer();
                CustomerBL cutomBL = new CustomerBL();

                try
                {
                    it = itBL.GetItemByID(itID);

                }
                catch (System.Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    throw;
                }
                if (it == null)
                {
                    Console.WriteLine("The item does not exist !");
                }
                else
                {
                    Console.WriteLine(row);
                    Console.WriteLine("DETAIL OF ITEM");
                    Console.WriteLine(row);
                    var table = new ConsoleTable("ID", "ITEM NAME", "PRICE OF SIZE M", "PRICE OF SIZE X", "PRICE OF SIZE XL");

                    table.AddRow(it.ItemID, it.ItemName, FomatMoney(it.ItemPrice), FomatMoney(it.ItemPrice + 5), FomatMoney(it.ItemPrice + 10));

                    table.Write();
                    Console.WriteLine("DESCRIPTION : ");
                    WriteLineWordWrap(it.ItemDescription);
                    
                }
                string select;
                Console.WriteLine("\n" + row + "\n");
                Console.WriteLine("1. Add to cart");
                Console.WriteLine("2. Back to Menu");
                Console.Write("Enter your selection: ");
                select = Console.ReadLine();

                switch (select)
                {
                    case "1":

                        while (true)
                        {
                            Console.Write("Enter size of item: ");
                            it.Size = Console.ReadLine().ToUpper();
                            if (it.Size != "M" && it.Size != "X" && it.Size != "XL")
                            {
                                Console.WriteLine("Size of item must be M,X,XL");
                                Console.ReadKey();
                                continue;
                            }
                            List<Items> litems = null;
                            if (File.Exists(("shoppingcart" + customer.UserName + ".dat")))
                            {
                                FileStream f = new FileStream("shoppingcart" + customer.UserName + ".dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                BinaryReader br = new BinaryReader(f);
                                string a = br.ReadString();
                                litems = JsonConvert.DeserializeObject<List<Items>>(a);
                                br.Close();
                                f.Close();
                            }
                            while (true)
                            {
                                string selection;
                                int temp = 0;
                                int index = 0;
                                if (litems != null)
                                {
                                    for (int i = 0; i < litems.Count; i++)
                                    {
                                        if (it.Size == litems[i].Size)
                                        {
                                            temp += 1;
                                            index = i;
                                            break;
                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                                if (temp == 1)
                                {
                                    Console.Write("This item already exists in the shopping cart, do you want increase  quantity ?(Y/N):");
                                    selection = Console.ReadLine().ToUpper();
                                    int add;
                                    switch (selection)
                                    {
                                        case "Y":
                                            while (true)
                                            {
                                                Console.Write("Enter the quantity you want to add: ");
                                                try
                                                {
                                                    add = int.Parse(Console.ReadLine());
                                                    litems[index].ItemCount += add;

                                                    string fileName = "shoppingcart" + customer.UserName + ".dat";
                                                    File.Delete(fileName);
                                                    string sJSONResponse = JsonConvert.SerializeObject(litems);
                                                    BinaryWriter bw;
                                                    try
                                                    {
                                                        FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                                        bw = new BinaryWriter(fs);
                                                        bw.Write((string)(object)sJSONResponse);
                                                        fs.Close();
                                                    }
                                                    catch (System.Exception)
                                                    {
                                                        Console.WriteLine("Disconnect from database !");
                                                    }
                                                    Console.WriteLine("\nAdd quantity success !\n");

                                                    break;
                                                }
                                                catch (System.Exception)
                                                {
                                                    Console.WriteLine("Item id must be integer and greater 0 !");
                                                    continue;
                                                }
                                            }

                                            MenuAfterLogin();
                                            break;
                                        case "N":
                                            MenuAfterLogin();
                                            break;
                                        default:
                                            Console.Write("You must enter Y/N \n");
                                            continue;
                                    }
                                }
                                if (it.Size != "M" && it.Size != "X" && it.Size != "XL")
                                {
                                    Console.WriteLine("Size of item must be M,X,XL");
                                    Console.ReadKey();
                                    continue;
                                }
                                else
                                {
                                    if (it.Size == "X")
                                    {
                                        it.ItemPrice += 5;
                                    }
                                    if (it.Size == "XL")
                                    {
                                        it.ItemPrice += 10;
                                    }
                                    break;
                                }
                            }
                            while (true)
                            {
                                int Count = 0;
                                Console.Write("Enter quantity of item:");
                                bool a = Int32.TryParse(Console.ReadLine(), out Count);
                                if (Count > 0 && Count < 50)
                                {
                                    it.ItemCount = Count;

                                    break;
                                }
                                else if (!a)
                                {
                                    Console.WriteLine("\nYou must enter integer and greater than 0 and less than 50 !\n");
                                    continue;
                                }
                                else
                                {
                                    Console.WriteLine("\nYou must enter integer and greater than 0 and less than 50 !\n");
                                    continue;
                                }

                            }
                            AddToCart(it);
                            break;
                        }


                        break;
                    case "2":
                        MenuAfterLogin();
                        break;
                    default:
                        Console.WriteLine("You are only entered in the number existing !");
                        continue;
                }

                string conti;
                Console.Write("Do you continue ? (Y/N): ");
                conti = checkYN().ToUpper();
                if (conti == "Y")
                {
                    itBL = new ItemBL();
                    it = new Items();
                    // li = null;
                    custom = new Customer();
                    cutomBL = new CustomerBL();
                    showItems();
                }
                else if (conti == "N")
                {
                    MenuAfterLogin();
                }


            }
        }
        public string Password()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        sb.Length--;
                    }
                    continue;
                }
                Console.Write('*');

                sb.Append(cki.KeyChar);
            }
            return sb.ToString();
        }
        public bool validate(string str)
        {
            Regex regex = new Regex("[a-zA-Z0-9_]");
            MatchCollection matchCollectionstr = regex.Matches(str);
            // Console.WriteLine(matchCollectionstr.Count);
            if (matchCollectionstr.Count < str.Length)
            {
                return false;
            }
            return true;

        }
        public bool validateChoice(string str)
        {
            Regex regex = new Regex("[0-9]");
            MatchCollection matchCollectionstr = regex.Matches(str);
            if (matchCollectionstr.Count < str.Length)
            {
                return false;
            }
            return true;

        }
        public void ViewInfo()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine(row);
                Console.WriteLine("CUSTOMER INFOMATION");
                Console.WriteLine(row);
                Console.WriteLine("Customer Id : {0}", customer.CusID);
                Console.WriteLine("Customer Username : {0}", customer.UserName);
                Console.WriteLine("Customer Name : {0}", customer.CusName);
                Console.WriteLine("Customer Address : {0}", customer.Address);
                Console.WriteLine("Customer Phone Number: {0}", customer.PhoneNumber);

                string choice = null;
                Console.WriteLine("\n" + row + "\n");
                Console.WriteLine("1. Show items");
                Console.WriteLine("2. Back");
                Console.Write("Please enter your selection: ");
                choice = Console.ReadLine();
                if (validateChoice(choice) == false)
                {
                    Console.Write("You are only entered in the number existing!");

                }

                switch (choice)
                {
                    case "1":
                        showItems();
                        break;
                    case "2":
                        break;
                    default:
                        Console.WriteLine("You are only entered in the number of existing ids !");
                        continue;
                }
                if (choice == "2")
                {
                    break;
                }
            }
        }
        public void LoginMenu()
        {

            CustomerBL cuBL = new CustomerBL();

            string userName = null;
            string password = null;
            while (true)
            {
                Console.Clear();
                string row = ("==================================================================");
                Console.WriteLine(row);
                Console.WriteLine();
                Console.WriteLine("LOGIN MENU \n");
                Console.WriteLine(row);
                Console.Write("Enter user name: ");
                userName = Console.ReadLine().Trim();
                Console.Write("Enter password: ");
                password = Password();
                if ((validate(userName) == false) || (validate(password) == false))
                {
                    Console.Write("User name / Password cannot contain special characters");
                    LoginMenu();
                }
                try
                {
                    customer = cuBL.GetCustomerByUsernameAndPassword(userName, password);
                }
                catch (System.Exception)
                {
                    // throw;
                    Console.WriteLine("Disconnect from database");
                    Console.ReadKey();
                    LoginMenu();
                }

                if (customer == null)
                {
                    Console.WriteLine("Username or password is incorrect!");
                }
                else
                {
                    MenuAfterLogin();
                }
            }
        }
        public void ShowBill(int orderId)
        {
            while (true)
            {
                OrderBL oBL = new OrderBL();
                Orders order = new Orders();
                try
                {
                    order = oBL.GetOrderDetailByOrderID(orderId);
                }
                catch (System.Exception ex)
                {

                    Console.WriteLine(ex);
                    Console.ReadKey();
                    break;
                }
                decimal AmountPay = 0;
                Console.Clear();
                Console.WriteLine(row);
                Console.WriteLine(" VTCA CAFFE");
                Console.WriteLine(row);
                Console.WriteLine("\n                           BILL                       \n");
                Console.WriteLine(" Order date        : {0}", order.OrderDate.ToString("dddd, dd MMMM yyyy"));
                Console.WriteLine(" Order ID          : {0}", order.OrderID);
                Console.WriteLine(" Customer name     : {0}", customer.CusName);

                var table = new ConsoleTable("ITEM NAME", "PRICE ", "QUANTITY", "SIZE", "TOTAL");
                foreach (var item in order.Items)
                {
                    if (item.Size == "X")
                    {
                        item.ItemPrice += 5;
                    }
                    if (item.Size == "XL")
                    {
                        item.ItemPrice += 10;
                    }
                    Decimal TOTAL = item.ItemPrice * item.ItemCount;
                    AmountPay += TOTAL;
                    table.AddRow(item.ItemName, FomatMoney(item.ItemPrice), item.ItemCount, item.Size, FomatMoney(TOTAL));

                }
                table.Write();
                Console.WriteLine("\n Amount : {0} (Including VAT) \n", AmountPay);
                Console.WriteLine("Press anykey to back");
                Console.ReadKey();
                break;
            }
        }
        public void MenuAfterLogin()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine(row);
                Console.WriteLine("WELCOME BACK ! {0}", customer.CusName.ToUpper());
                string chose;
                Console.WriteLine(row);
                Console.WriteLine("1. View Infomation");
                Console.WriteLine("2. Show Items");
                Console.WriteLine("3. Show Cart");
                Console.WriteLine("4. Show bought order");
                Console.WriteLine("5. Log out");
                Console.Write("Enter your selction: ");
                chose = Console.ReadLine();
                switch (chose)
                {
                    case "1":
                        ViewInfo();
                        break;
                    case "2":
                        showItems();
                        break;
                    case "3":

                        ShowCart();
                        break;

                    case "4":
                        ShowOrderBought(customer.CusID);
                        break;
                    case "5":
                        MainMenu();
                        break;
                    default:
                        Console.WriteLine("Input invalid !");
                        continue;
                }
            }
        }
        public string FomatMoney(decimal money)
        {
            string a;
            a = money.ToString("C3", CultureInfo.CurrentCulture);
            return a;
        }
        public static void WriteLineWordWrap(string paragraph, int tabSize = 8)
        {
            string[] lines = paragraph
                .Replace("\t", new String(' ', tabSize))
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string process = lines[i];
                List<String> wrapped = new List<string>();

                while (process.Length > Console.WindowWidth)
                {
                    int wrapAt = process.LastIndexOf(' ', Math.Min(Console.WindowWidth - 1, process.Length));
                    if (wrapAt <= 0) break;

                    wrapped.Add(process.Substring(0, wrapAt));
                    process = process.Remove(0, wrapAt + 1);
                }

                foreach (string wrap in wrapped)
                {
                    Console.WriteLine(wrap);
                }

                Console.WriteLine(process);
            }
        }

    }
}

