// using System;
// using System.Text;
// using System.Text.RegularExpressions;
// using BL;
// using Persistence.Model;
// using System.Security;
// using System.Collections.Generic;
// using ConsoleTables;
// using System.IO;
// using Newtonsoft.Json;

// namespace PL_Console
// {
//     public class Functions
//     {
//         private Customers cusAll = new Customers();
//         CustomerBL cuBL = new CustomerBL();
//         ItemBL itemBL = new ItemBL();
//         Orders order = new Orders();
//         List<Items> items = new List<Items>();
//         OrderBL orderBL = new OrderBL();
//         Items item = new Items();
//         private string row = ("==================================================================");
//         public void LoginMenu()
//         {
//             // Console.Clear();
//             CustomerBL cuBL = new CustomerBL();
//             Customers cu = new Customers();
//             string userName = null;
//             string password = null;
//             string choice;
//             while (true)
//             {

//                 Console.WriteLine(row);
//                 Console.WriteLine();
//                 Console.WriteLine("LOGIN MENU \n");
//                 Console.WriteLine(row);
//                 Console.Write("Enter user name: ");
//                 userName = Console.ReadLine().Trim();
//                 Console.Write("Enter password: ");
//                 password = Password().Trim();
//                 try
//                 {
//                     cusAll = cu = cuBL.Login(userName, password);
//                 }
//                 catch (System.Exception)
//                 {
//                     Console.WriteLine("Disconnect from database!");
//                     LoginMenu();
//                 }
//                 if ((validate(userName) == false) || (validate(password) == false))
//                 {
//                     Console.Write("User name / Password cannot contain special characters, do you want re-enter ? (Y/N)");
//                     choice = checkYN();
//                     switch (choice)
//                     {
//                         case "Y":
//                             continue;
//                         case "N":
//                             showItems();
//                             break;
//                         default:
//                             continue;
//                     }
//                 }
//                 if (cu == null)
//                 {
//                     Console.WriteLine("Username or password is incorrect!");
//                 }
//                 else
//                 {
//                     MenuAfterLogin();
//                 }
//             }
//         }
//         public void MenuAfterLogin()
//         {
//             while (true)
//             {


//                 Console.WriteLine(row);
//                 Console.WriteLine("WELCOME BACK ! {0}", cusAll.CusName.ToUpper());
//                 string chose;
//                 Console.WriteLine(row);
//                 Console.WriteLine("1. View Infomation");
//                 Console.WriteLine("2. Go to menu");
//                 Console.WriteLine("3. Show Cart");
//                 Console.WriteLine("4. log out");
//                 Console.Write("Enter your selction: ");
//                 chose = Console.ReadLine();


//                 switch (chose)
//                 {
//                     case "1":
//                         ViewInfo(cusAll.UserName, cusAll.Password);
//                         break;
//                     case "2":
//                         showItems();
//                         break;
//                     case "3":

//                         ShowCart();
//                         break;

//                     case "4":
//                         LoginMenu();
//                         break;

//                     default:
//                         Console.WriteLine("Input invalid !");
//                         continue;
//                 }
//             }
//         }
//         public void ViewInfo(string userName, string Password)
//         {
//             while (true)
//             {

//                 string row = ("==================================================================");
//                 Console.WriteLine(row);
//                 Console.WriteLine("CUSTOMER INFOMATION");
//                 Console.WriteLine(row);
//                 Console.WriteLine("Customer Id : {0}", cusAll.CusID);
//                 Console.WriteLine("Customer Username : {0}", cusAll.UserName);
//                 Console.WriteLine("Customer Name : {0}", cusAll.CusName);
//                 Console.WriteLine("Customer Address : {0}", cusAll.Address);
//                 Console.WriteLine("Customer Phone Number: {0}", cusAll.PhoneNumber);
//                 string choice = null;
//                 Console.WriteLine("\n" + row + "\n");
//                 Console.WriteLine("1. Go to menu");
//                 Console.WriteLine("2. Back");
//                 Console.Write("Please enter your selection: ");
//                 choice = Console.ReadLine();
//                 if (validateChoice(choice) == false)
//                 {
//                     Console.Write("You are only entered in the number existing!");

//                 }
//                 switch (choice)
//                 {
//                     case "1":
//                         showItems();
//                         break;
//                     case "2":


//                         break;
//                     default:
//                         Console.WriteLine("You are only entered in the number of existing ids !");
//                         continue;
//                 }
//                 if (choice == "2")
//                 {
//                     break;
//                 }
//             }
//         }
//         public string Password()
//         {
//             StringBuilder sb = new StringBuilder();
//             while (true)
//             {
//                 ConsoleKeyInfo cki = Console.ReadKey(true);
//                 if (cki.Key == ConsoleKey.Enter)
//                 {
//                     Console.WriteLine();
//                     break;
//                 }
//                 if (cki.Key == ConsoleKey.Backspace)
//                 {
//                     if (sb.Length > 0)
//                     {
//                         Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
//                         Console.Write(" ");
//                         Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
//                         sb.Length--;
//                     }
//                     continue;
//                 }
//                 Console.Write('*');

//                 sb.Append(cki.KeyChar);
//             }
//             return sb.ToString();
//         }


//         public bool validate(string str)
//         {
//             Regex regex = new Regex("[a-zA-Z0-9_]");
//             MatchCollection matchCollectionstr = regex.Matches(str);
//             if (matchCollectionstr.Count < str.Length)
//             {
//                 return false;
//             }
//             return true;
//         }


//         public void showItems()
//         {
//             string row = ("==================================================================");
//             Console.WriteLine(row);
//             Console.WriteLine("MENU");
//             Console.WriteLine(row);
//             Items it = new Items();
//             try
//             {
//                 items = itemBL.GetItems();
//             }
//             catch (System.Exception)
//             {
//                 Console.WriteLine("Disconnect from datebase !");
//                 Environment.Exit(0);
//             }
//             var table = new ConsoleTable("ID", "ITEM NAME");
//             foreach (var item in items)
//             {
//                 table.AddRow(item.ItemID, item.ItemName);
//             }
//             table.Write();

//             showItemDetail(items);
//         }

//         public void showItemDetail(List<Items> li)
//         {
//             while (true)
//             {
//                 string row = ("==================================================================");
//                 string choice;
//                 int itID;
//                 try
//                 {
//                     li = itemBL.GetItems();
//                     Console.Write("Enter item id: ");
//                     itID = int.Parse(Console.ReadLine());
//                 }
//                 catch (System.Exception)
//                 {

//                     Console.WriteLine("Item id must be integer and in the options !");
//                     continue;
//                 }

//                 if (itID > li.Count || validateChoice(itID.ToString()) == false)
//                 {
//                     Console.Write("You are only entered in the number of existing ids !");

//                     while (true)
//                     {
//                         Console.Write("Do  you want re-enter ? (Y/N): ");
//                         choice = Console.ReadLine().ToUpper();
//                         if (choice != "Y" && choice != "N")
//                         {
//                             Console.Write("You can only enter  (Y/N): ");
//                             choice = Console.ReadLine().ToUpper();
//                             continue;
//                         }
//                         break;

//                     }

//                     switch (choice)
//                     {
//                         case "Y":
//                             continue;

//                         case "N":
//                             showItems();
//                             break;

//                         default:
//                             continue;
//                     }
//                 }

//                 try
//                 {
//                     item = itemBL.GetItemByID(itID);
//                 }
//                 catch (System.Exception)
//                 {

//                     Console.WriteLine("Disconnect from database !");
//                 }
//                 if (item == null)
//                 {
//                     Console.WriteLine("The item does not exist !");
//                 }
//                 else
//                 {
//                     Console.WriteLine(row);
//                     Console.WriteLine("DETAIL OF ITEM");
//                     Console.WriteLine(row);
//                     var table = new ConsoleTable("ID", "ITEM NAME", "ITEM PRICE", "SIZE");

//                     table.AddRow(item.ItemID, item.ItemName, item.ItemPrice, item.Size);

//                     table.Write();
//                     Console.WriteLine("DESCRIPTION : ");
//                     Console.WriteLine(item.ItemDescription);
//                 }
//                 string select;
//                 Console.WriteLine("\n" + row + "\n");
//                 Console.WriteLine("1. Add to cart");
//                 Console.WriteLine("2. Back to Menu");
//                 Console.Write("Enter your selection: ");
//                 select = Console.ReadLine();

//                 switch (select)
//                 {
//                     case "1":
//                         AddToCart(item);
//                         break;
//                     case "2":
//                         break;
//                     default:
//                         Console.WriteLine("You are only entered in the number existing !");
//                         continue;
//                 }
//                 if (select == "2")
//                 {
//                     break;
//                 }
//                 string conti;
//                 Console.Write("Do you continue ? (Y/N): ");
//                 conti = checkYN();
//                 if (conti == "Y")
//                 {
//                     showItems();
//                 }
//                 else
//                 {
//                     break;
//                 }


//             }
//         }
//         public void AddToCart(Items item)
//         {


//             items.Add(item);
//             string sJSONResponse = JsonConvert.SerializeObject(item);
//             BinaryWriter bw;
//             string fileName = "shoppingcart" + cusAll.UserName + ".dat";
//             try
//             {
//                 FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
//                 bw = new BinaryWriter(fs);
//                 bw.Write((string)(object)sJSONResponse);
//                 fs.Close();
//             }
//             catch (System.Exception)
//             {
//                 Console.WriteLine("Disconnect from database !");
//             }
//             Console.WriteLine("Add to cart success !");
//         }
//         public void ShowCart()
//         {
//             while (true)
//             {
//                 if (File.Exists("shoppingcart" + cusAll.UserName + ".dat"))
//                 {
//                     List<Items> itemsa = null;
  
//                     decimal amount = 0;
//                     try
//                     {
//                         FileStream fs = new FileStream("shoppingcart" + cusAll.UserName + ".dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
//                         BinaryReader br = new BinaryReader(fs);
//                         string a = br.ReadString();
                      
//                         itemsa = JsonConvert.DeserializeObject<List<Items>>(a);
                     
//                         br.Close();
//                         fs.Close();
//                     }
//                     catch (System.Exception)
//                     {
//                         throw;
//                     }
//                     var table = new ConsoleTable("ID", "ITEM NAME");
//                     foreach (Items itema in itemsa)
//                     {
//                         table.AddRow(itema.ItemID, itema.ItemName);
//                         amount += itema.ItemPrice;
//                     }
//                     table.Write();
//                     string orde;
//                     Console.Write("Do you want create order ? (Y/N):");
//                     orde = checkYN();
//                     if (orde == "Y")
//                     {
//                         Console.Write("Enter your note: ");
//                         string note = Console.ReadLine();
//                         order.Note = note;
//                         DateTime date = DateTime.Now;
//                         order.OrderDate = date;
//                         order.Status = "Not yet";
//                         order.Amount = amount;
//                         order.CustomerID = cusAll;

//                         foreach (Items item in itemsa)
//                         {
//                             order.Items = new List<Items>();
//                             // or.Items.Add(itBl.GetItemByItemID(or.ItemID));
//                             order.Items.Add(itemBL.GetItemByID(item.ItemID));
//                         }
//                         bool a = true;

//                         a = orderBL.CreateOrder(order);

//                         if (a == true)
//                         {
//                             Console.WriteLine("Create order success ! ");
//                             try
//                             {
//                                 // Check if file exists with its full path    
//                                 if (File.Exists(Path.Combine("shoppingcart" + cusAll.UserName + ".dat")))
//                                 {
//                                     // If file found, delete it    
//                                     File.Delete(Path.Combine("shoppingcart" + cusAll.UserName + ".dat"));
//                                 }
//                                 else Console.WriteLine("Cart not found");
//                             }
//                             catch (IOException ioExp)
//                             {
//                                 Console.WriteLine(ioExp.Message);
//                             }
//                             break;
//                         }
//                         else
//                         {
//                             Console.WriteLine("\n ☹  Create order faild , press anykey to continue !\n");
//                             Console.ReadKey();
//                             break;
//                         }
//                     }
//                     else
//                     {
//                         break;
//                     }

//                 }
//                 else
//                 {
//                     Console.WriteLine("\nNo shopping cart yet , press anykey to continue !\n");
//                     Console.ReadKey();
//                     break;
//                 }
//             }

//         }


//         public string checkYN()
//         {
//             string choice = Console.ReadLine().ToUpper();
//             while (true)
//             {
//                 if (choice != "Y" && choice != "N")
//                 {
//                     Console.Write("You can only enter (y / s): ");
//                     choice = Console.ReadLine().ToUpper();
//                     continue;
//                 }
//                 break;
//             }
//             switch (choice)
//             {
//                 case "Y":
//                     break;
//                 case "N":
//                     break;

//             }
//             return choice;
//         }
//         public bool validateChoice(string str)
//         {
//             Regex regex = new Regex("[0-9]");
//             MatchCollection matchCollectionstr = regex.Matches(str);
//             // Console.WriteLine(matchCollectionstr.Count);
//             if (matchCollectionstr.Count < str.Length)
//             {
//                 return false;
//             }
//             return true;

//         }
//     }
// }