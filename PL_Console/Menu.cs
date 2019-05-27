using System;
using System.Text;
using System.Text.RegularExpressions;
using BL;
using Persistence.Model;
using System.Security;
using System.Collections.Generic;
using ConsoleTables;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
namespace PL_Console
{
    public class Menu
    {
        string row = ("==================================================================");
        decimal amount = 0;

        private Customer customer = new Customer();
        private List<Items> items = new List<Items>();




        public void MainMenu()
        {
            while (true)
            {
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
                    Console.Write("You can only enter (y / s): ");
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
        public void Pay()
        {
            CustomerBL cuBL = new CustomerBL();
            OrderBL ordersbl = new OrderBL();
            Orders or = new Orders();
            or.Customer = new Customer();

            while (true)
            {

                decimal? Totle;
                Console.WriteLine(row);
                Console.WriteLine("PAY");
                Console.WriteLine(row);
                Console.WriteLine("The money you must pay : " + amount);
                try
                {
                    Totle = cuBL.GetMoneybyCustomerId(customer.CusID);
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Disconnect from database  !");
                    continue;
                }
                Console.WriteLine("Amount you have: " + Totle);
                Console.WriteLine("Press anykey to pay !");
                Console.ReadKey();


                if (Totle < amount)
                {
                    Console.WriteLine("The amount you must pay is greater than the amount you have !");
                    break;
                }
                else
                {
                    try
                    {
                        bool UpdateMoney = cuBL.UpdateMoneyCustomer(customer.CusID, amount);

                        bool UpdateStatus = ordersbl.UpdateStatus(or.OrderID);
                    }
                    catch (System.Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                        Console.ReadKey();
                        break;
                    }

                    Console.WriteLine("Pay success !");
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

                    Console.WriteLine("\n  Pay success ! press anykey to continue !\n");
                    Console.ReadKey();
                    break;
                }


            }
        }
        public void AddToCart(Items item)
        {
            Customer cust = new Customer();
            items.Add(item);
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
            catch (System.Exception)
            {

                Console.WriteLine("Disconnect from database !");
            }
            Console.WriteLine("\nAdd to cart success !\n");
        }
        public void ShowCart()
        {

            while (true)
            {
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
                    catch (System.Exception)
                    {
                        throw;
                    }
                    var table = new ConsoleTable("ID", "ITEM NAME","QUANTITY");
                    foreach (Items itema in itemsa)
                    {

                        table.AddRow(itema.ItemID, itema.ItemName,itema.ItemCount);
                        amount += itema.ItemPrice * itema.ItemCount;

                    }
                    // Console.WriteLine(amount);
                    table.Write();
                    string orde;
                    Console.Write("Do you want create order ? (Y/N):");
                    orde = checkYN();
                    if (orde == "Y")
                    {
                        OrderBL ordersbl = new OrderBL();
                        Console.Write("Enter your note: ");
                        string note = Console.ReadLine();
                        or.Note = note;
                        DateTime date = DateTime.Now;
                        or.OrderDate = date;
                        or.Status = "Not yet";
                        or.Amount = amount;
                        or.Customer = customer;
                        ItemBL itBl = new ItemBL();

                        foreach (Items item in itemsa)
                        {
                            or.Items = new List<Items>();
                            or.Items.Add(itBl.GetItemByID(item.ItemID));
                        }
                        bool a = true;

                        a = ordersbl.CreateOrder(or);
                        if (a == true)
                        {
                            string choice = null;
                            Console.WriteLine("Create order success ! ");

                            Console.WriteLine("1. Pay now");
                            Console.WriteLine("2. Cancel order");
                            Console.Write("Enter your select : ");
                            choice = Console.ReadLine();
                            switch (choice)
                            {
                                case "1":
                                    Pay();
                                    break;
                                case "2":
                                    a = ordersbl.DeleteOrder(or.OrderID);
                                    Console.WriteLine("Order has been canceled");
                                    continue;
                                default:
                                    Console.WriteLine("input invalid !");
                                    continue;
                            }

                            break;
                        }
                        if (a == false)
                        {
                            Console.WriteLine("\n ☹  Create order faild , press anykey to continue !\n");
                            Console.ReadKey();
                            break;
                        }
                    }
                    else
                    {
                        break;
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
        public void ShowOrderBought(int CustomerId)
        {

            while (true)
            {
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
                    break;
                }
                if (orders == null)
                {
                    Console.WriteLine("List empty, press anykey to continue !\n");
                    Console.ReadKey();
                    break;
                }
                var table = new ConsoleTable("ORDER ID", "ORDER DATE", "NOTE", "ORDER STATUS");
                foreach (Orders ord in orders)
                {
                    table.AddRow(ord.OrderID, ord.OrderDate, ord.Note, ord.Status);
                }
                table.Write();
                Console.WriteLine("\nPress anykey to continue !\n");
                Console.ReadKey();
                break;
            }
        }

        public void showItems()
        {
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
            var table = new ConsoleTable("ID", "ITEM NAME");
            foreach (Items item in li)
            {
                table.AddRow(item.ItemID, item.ItemName);
            }
            table.Write();
            showItemDetail();
        }

        public void showItemDetail()
        {
            while (true)
            {

                string row = ("==================================================================");


                ItemBL itBL = new ItemBL();
                Items it = new Items();
                List<Items> li = null;
                Customer custom = new Customer();
                CustomerBL cutomBL = new CustomerBL();
               int itemCount;

                string choice;
                int itID;
                while (true)
                {


                    try
                    {
                        li = itBL.GetItems();
                        Console.Write("Enter item id: ");
                        itID = int.Parse(Console.ReadLine());
                        Console.Write("Enter number of item:");
                        itemCount = int.Parse(Console.ReadLine());


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


                try
                {
                    it = itBL.GetItemByID(itID);
                    it.ItemCount = itemCount;
                    amount += it.ItemCount * it.ItemPrice;
                    Console.WriteLine(amount);
                
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
                    var table = new ConsoleTable("ID", "ITEM NAME", "ITEM PRICE", "SIZE");

                    table.AddRow(it.ItemID, it.ItemName, it.ItemPrice, it.Size);

                    table.Write();
                    Console.WriteLine("DESCRIPTION : ");
                    Console.WriteLine(it.ItemDescription);
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
                        AddToCart(it);
                        break;
                    case "2":
                        break;
                    default:
                        Console.WriteLine("You are only entered in the number existing !");
                        continue;
                }
                if (select == "2")
                {
                    break;
                }
                string conti;
                Console.Write("Do you continue ? (Y/N): ");
                conti = checkYN().ToUpper();
                if (conti == "Y")
                {
                    showItems();
                }
                if (conti == "N")
                {
                    break;
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
                Console.WriteLine("1. Go to menu");
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
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
                Console.WriteLine("2. Go to menu");
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
                        LoginMenu();
                        break;
                    default:
                        Console.WriteLine("Input invalid !");
                        continue;
                }
            }
        }
    }
}

