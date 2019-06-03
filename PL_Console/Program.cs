using System;

namespace PL_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("=================== WELCOME TO VTCA CAFFE !=======================");
            Menu menu = new Menu();
            menu.MainMenu();
        }
    }
}
