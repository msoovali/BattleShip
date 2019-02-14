using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using DAL;
using Domain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp
{
    public class ApplicationMenu
    {
        public static Menu InitMenu { get; set; }
        public static Menu PlayerMenu { get; set; }
        public static Menu MainMenu { get; set; }
        public static Menu ShipMenu { get; set; }
        public static Menu ShipPlacementMenu { get; set; }
        public Game MyGame { get; set; } = new Game();

        public DbContext Ctx { get; set; }
        
        public void StartMenu()
        {
            ShipPlacementMenu = new Menu()
            {
                Title = "Choose how ships are going to be placed!"
            };
            
            ShipPlacementMenu.MenuItemsDictionary.Add(
                "C", new MenuItem()
                {
                    Description = "Custom",
                    ActionToRun = PlaceShipsOnBoard
                });
            
            ShipPlacementMenu.MenuItemsDictionary.Add(
                "R", new MenuItem()
                {
                    Description = "Random",
                    ActionToRun = PlaceShipsRandomly
                });
            
            ShipMenu = new Menu()
            {
                Title = "Choose Your ships sizes!"
            };
            
            ShipMenu.MenuItemsDictionary.Add(
                "S", new MenuItem()
                {
                    Description = "Standard (5 ships with sizes 1,2,3,4,5)",
                    ActionToRun = SetStandardShips
                });
            ShipMenu.MenuItemsDictionary.Add(
                "C", new MenuItem()
                {
                    Description = "Custom",
                    ActionToRun = ChangeShipsCount
                });
            MainMenu = new Menu()
            {
                Title = "Main Menu\n***To change gamemode go back***"
            };
            
            MainMenu.MenuItemsDictionary.Add(
                "S", new MenuItem()
                {
                    Description = "Start game!",
                    ActionToRun = ShipMenu.RunMenu
                });
            
            MainMenu.MenuItemsDictionary.Add(
                "B", new MenuItem()
                {
                    Description = "Change board size",
                    ActionToRun = ChangeBoardSize
                });
            MainMenu.MenuItemsDictionary.Add(
                "T", new MenuItem()
                {
                    Description = "Turn ships can touch On/Off",
                    ActionToRun = ChangeShipsTouch
                });
            MainMenu.MenuItemsDictionary.Add(
                "D", new MenuItem()
                {
                    Description = "Display current settings!",
                    ActionToRun = CurrentSettings
                });
            PlayerMenu = new Menu()
            {
                Title = "Choose game mode!"
            };
            
            PlayerMenu.MenuItemsDictionary.Add(
                "1", new MenuItem()
                {
                    Description = "Single-player",
                    ActionToRun = SetName
                });
            
            PlayerMenu.MenuItemsDictionary.Add(
                "2", new MenuItem()
                {
                    Description = "Two-player",
                    ActionToRun = SetNames
                });
            
            InitMenu = new Menu()
            {
                Title = "Welcome to game of battleship!"
            };

            InitMenu.MenuItemsDictionary.Add(
                "N", new MenuItem()
                {
                    Description = "New game",
                    ActionToRun = PlayerMenu.RunMenu
                }
            );
            
            InitMenu.MenuItemsDictionary.Add(
                "L", new MenuItem()
                {
                    Description = "Load game",
                    ActionToRun = LoadGame
                }
            );
            InitMenu.RunMenu();
        }

        private void LoadGame()
        {
            Console.Clear();
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySQL(
                "server=alpha.akaver.com;database=student2018_masoov;user=student2018;password=student2018");
            var ctx = new AppDbContext(optionsBuilder.Options);
            int gameId;
            do
            {
                Console.WriteLine("Enter previous game id!");
                gameId = ConsoleView.IntegerInputHelper("Game ID", 0);
                try
                {
                    MyGame = ctx.Games.First(g => g.GameId == gameId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                if (MyGame.GameOver)
                {
                    Console.WriteLine($"Game has already ended, please load other game!");
                    ConsoleView.WaitForUser();
                    continue;
                }

                break;
            } while (true);
            ActionMenu.BombingMenu(MyGame, ctx);
        }
        private void PlaceShipsRandomly()
        {
            InitBoard.InitializeBoard(MyGame);
            ShipPlacement.RandomPlacement(MyGame, false);
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySQL(
                "server=alpha.akaver.com;database=student2018_masoov;user=student2018;password=student2018");
            Ctx = new AppDbContext(optionsBuilder.Options);
            Ctx.Add(MyGame);
            ActionMenu.BombingMenu(MyGame, Ctx);
        }
        private void SetStandardShips()
        {
            MyGame.GameShips = new List<Ship>()
            {
                new Ship(5),
                new Ship(4),
                new Ship(3),
                new Ship(2),
                new Ship(1)
            };
            ShipPlacementMenu.RunMenu();
        }

        private void PlaceShipsOnBoard()
        {
            Console.WriteLine($"{MyGame.PlayerOne}, place your ships!");
            ConsoleView.WaitForUser();
            ConsoleView.PlacePlayerShipsOnBoard(MyGame);
            if (MyGame.Ai)
            {
                ShipPlacement.RandomPlacement(MyGame, true);
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"{MyGame.PlayerTwo}, place your ships!");
                ConsoleView.WaitForUser();
                MyGame.PlayerOneTurn = false;
                ConsoleView.PlacePlayerShipsOnBoard(MyGame);
                MyGame.PlayerOneTurn = true;
            }
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseMySQL(
                "server=alpha.akaver.com;database=student2018_masoov;user=student2018;password=student2018");
            Ctx = new AppDbContext(optionsBuilder.Options);
            Ctx.Add(MyGame);
            ActionMenu.BombingMenu(MyGame, Ctx);
        }

        private void ChangeShipsTouch()
        {
            Console.Clear();
            MyGame.ShipsCanTouch = !MyGame.ShipsCanTouch;
            Console.WriteLine($"Ships can touch set to: {MyGame.ShipsCanTouch}");
            ConsoleView.WaitForUser();
        }
        
        private void ChangeBoardSize()
        {   
            Console.Clear();
            MyGame.Rows = ConsoleView.IntegerInputHelper("rows", MyGame.Rows);
            Console.Clear();
            MyGame.Cols = ConsoleView.IntegerInputHelper("cols", MyGame.Cols);
            Console.WriteLine($"Board set to: {MyGame.Rows}x{MyGame.Cols}");
            ConsoleView.WaitForUser();
        }
        private void ChangeShipsCount()
        {
            int input;
            do
            {
                Console.Clear();
                input = ConsoleView.IntegerInputHelper("ships", MyGame.NumberOfShips);
                if (input > MyGame.Rows && input > MyGame.Cols)
                {
                    Console.WriteLine("There cannot be more ships than there are rows and columns on board!");
                    ConsoleView.WaitForUser();
                    continue;
                }
                break;
            } while (true);

            MyGame.NumberOfShips = input;
            Console.WriteLine($"No of ships set to: {MyGame.NumberOfShips}");
            ConsoleView.WaitForUser();
            MyGame.GameShips = new List<Ship>();
            
            for (int i = 1; i <= MyGame.NumberOfShips; i++)
            {
                Console.Clear();
                Console.Write($"Enter size of {i}. ship: ");
                do
                {
                    input = ConsoleView.IntegerInputHelper("ship size", MyGame.NumberOfShips);
                    if (input > MyGame.Cols && input > MyGame.Rows)
                    {
                        Console.WriteLine("Ship cannot be bigger than board!\nPlease try again:");
                        continue;
                    }
                    break;
                } while (true);
                MyGame.GameShips.Add(new Ship(input));
            }
            ShipPlacementMenu.RunMenu();
        }

        private void CurrentSettings()
        {
            Console.WriteLine($"Player 1: {MyGame.PlayerOne}\n" +
                              $"Player 2: {MyGame.PlayerTwo}\n" +
                              $"Board size: {MyGame.Rows}x{MyGame.Cols}\n" +
                              $"Ships can touch: {MyGame.ShipsCanTouch}");
            ConsoleView.WaitForUser();
        }
        private void SetName()
        {
            MyGame.Ai = true;
            MyGame.PlayerTwo = "AI";
            SetNickname(1);
            
        }
        private void SetNames()
        {
            MyGame.Ai = false;
            SetNickname(2);
            
        }
        private void SetNickname(int playerCount)
        {
            for (var i = 1; i <= playerCount; i++)
            {
                string input;
                do
                {
                    Console.Clear();
                    Console.Write($"Enter name for {i}. player: ");
                    input = Console.ReadLine();
                } while (string.IsNullOrEmpty(input));

                if (i == 1)
                {
                    MyGame.PlayerOne = input;
                }
                else
                {
                    MyGame.PlayerTwo = input;
                }
            }
            MainMenu.RunMenu();
            
         }
    }
}