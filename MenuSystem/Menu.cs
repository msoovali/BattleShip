using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace MenuSystem
{
    public class Menu
    {
        public Dictionary<string, MenuItem> MenuItemsDictionary { get; } = new Dictionary<string, MenuItem>()
        {
            {
                "Q", new MenuItem()
                {
                    Description = "Back",
                    MenuType = MenuType.GoBackOneLevel
                }
            }
        };
        public string Title { get; set; }

        private KeyValuePair<string, MenuItem> _goBackOneLevelItem;
        private void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine(Title);
            foreach (var menuItem in MenuItemsDictionary.Where(m => m.Value.MenuType == MenuType.Regular))
            {
                if (menuItem.Value.IsDefaultChoice)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine($"{menuItem.Key}) {menuItem.Value}");

                Console.ResetColor();
            }

            Console.WriteLine("------------------------");

            Console.WriteLine($"{_goBackOneLevelItem.Key}) {_goBackOneLevelItem.Value}");

            var defaultItem = MenuItemsDictionary.FirstOrDefault(item => item.Value.IsDefaultChoice);
            if (defaultItem.Value == null)
            {
                Console.Write(">");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"[{defaultItem.Key}]>");
                Console.ResetColor();
            }
        }
        private void WaitForUser()
        {
            Console.Write("Press any key to continue!");
            Console.ReadKey();
        }
        public void RunMenu()
        {
            _goBackOneLevelItem = 
                // single - return element if there is only 1 (not 0 or more than 1)
                MenuItemsDictionary.Single(m => m.Value.MenuType == MenuType.GoBackOneLevel);
            string menuCommand;
            do
            {
                ShowMenu();
                menuCommand = Console.ReadLine()?.ToUpper().Trim();
                if (menuCommand == _goBackOneLevelItem.Key) continue;

                MenuItem menuItem = null;

                if (string.IsNullOrWhiteSpace(menuCommand))
                {
                    menuItem = MenuItemsDictionary.FirstOrDefault(item => item.Value.IsDefaultChoice).Value;
                }
                else
                {
                    menuItem = MenuItemsDictionary.FirstOrDefault(item => item.Key == menuCommand).Value;
                }

                if (menuItem == null)
                {
                    Console.WriteLine("Command not found!");
                    WaitForUser();
                    continue;
                }
                if (menuItem?.ActionToRun == null)
                {
                    Console.WriteLine("This item has no action!");
                    WaitForUser();
                    continue;
                }
                menuItem.ActionToRun();
                // WaitForUser();

            } while (menuCommand != _goBackOneLevelItem.Key);
        }
    }
}