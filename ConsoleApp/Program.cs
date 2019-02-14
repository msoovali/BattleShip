using DAL;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var consoleMenu = new ApplicationMenu();
            consoleMenu.StartMenu();
        }
    }
}