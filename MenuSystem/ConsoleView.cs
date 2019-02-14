using System;
using System.Text;
using BLL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace MenuSystem
{
    public static class ConsoleView
    {
        private static string PrintBoard(Game game)
        {
            var sb = new StringBuilder();
            var board = game.PlayerTwoBoard;
            if (game.PlayerOneTurn)
            {
                board = game.PlayerOneBoard;
            }

            sb.Append("    ");
            for (var g = 0; g < game.Cols; g++)
            {
                sb.Append(" ");
                sb.Append($"{g}".PadRight(3));
            }
            for (var i = 0; i < game.Rows; i++)
            {
                sb.Append($"\n{i}".PadRight(3));
                sb.Append(" |");
                for (var j = 0; j < game.Cols; j++)
                {
                    switch (board[i][j])
                    {
                        case CellState.Empty:
                            sb.Append(" ~ |");
                            break;
                        case CellState.Miss:
                            sb.Append(" M |");
                            break;
                        case CellState.Hit:
                            sb.Append(" H |");
                            break;
                        case CellState.Ship:
                            sb.Append(" S |");
                            break;
                        case CellState.Wreck:
                            sb.Append(" W |");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return sb.ToString();
        }
        
        public static void WaitForUser()
        {
            Console.WriteLine("\nPress spacebar to continue!");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar){}
        }
        
        public static void PlacePlayerShipsOnBoard(Game game)
        {
            InitBoard.InitializeBoard(game);
            var count = 1;
            foreach (var ship in game.GameShips)
            {
                Console.Clear();
                Console.WriteLine(PrintBoard(game));
                string input;
                do
                {
                    Console.WriteLine($"Ships remaining: {game.NumberOfShips - count + 1}");
                    do
                    {
                        Console.WriteLine($"Current ship length: {ship.ShipLength}");
                        Console.WriteLine("Set layout: V vertical(|) or H horizontal(-)");
                        input = Console.ReadLine()?.Trim().ToUpper();
                        if (input == "H" || input == "V")
                        {
                            break;
                        }
                        Console.WriteLine("Invalid input!");
                    } while (true);

                    ship.ShipLayout = ShipLayout.Horizontal;
                    if (input == "V")
                    {
                        ship.ShipLayout = ShipLayout.Vertical;
                    }
                    Console.WriteLine("Set coordinates for this ship" +
                                      $" (size: {ship.ShipLength}, layout: {ship.ShipLayout}):");
                    Console.WriteLine("Coordinate row (left side number): ");
                    var x = IntegerInputHelper("x", ship.ShipLength, ship.ShipLayout, game);
                    Console.WriteLine("Coordinate column (top number): ");
                    var y = IntegerInputHelper("y", ship.ShipLength, ship.ShipLayout, game);
                    if (!ShipPlacement.CanPlaceShip(game, ship, x, y))
                    {
                        Console.WriteLine("Other ship at coordinates!");
                        continue;
                    }
                    
                    InitBoard.PlaceShip(game, ship, x, y);
                    Console.WriteLine("Ship placed successfully!");
                    break;
                } while (true);
                
                count++;
            }
        }
        public static int IntegerInputHelper(string str, int current, ShipLayout shipLayout = ShipLayout.Horizontal, Game game = null)
        {
            if (str == "ships" || str == "rows" || str == "cols")
            {
                Console.Clear();
            }
            int input;
            do
            {
                if (str == "ships" || str == "rows" || str == "cols")
                {
                    Console.WriteLine($"Current {str}: {current}");
                    Console.WriteLine($"Enter number of {str}: ");
                }
                try
                {
                    input = int.Parse(Console.ReadLine()?.Trim());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Please try again!");
                    continue;
                }

                if (str == "x")
                {
                    if (shipLayout == ShipLayout.Vertical && (0 > input || input > game.Rows - current))
                    {
                        Console.WriteLine($"LEFT coordinate has to be between 0 and {game.Rows - current}");
                        continue;
                    }
                    if (0 > input || input >= game.Rows)
                    {
                        Console.WriteLine($"LEFT coordinate has to be between 0 and {game.Rows - 1}");
                        continue;
                    }
                }
                
                if (str == "y")
                {
                    if (shipLayout == ShipLayout.Horizontal && (0 > input || input > game.Cols - current))
                    {
                        Console.WriteLine($"TOP coordinate has to be between 0 and {game.Cols - current}");
                        continue;
                    }

                    if ((0 > input || input >= game.Cols))
                    {
                        Console.WriteLine($"TOP coordinate has to be between 0 and {game.Cols - 1}");
                        continue;
                    }
                }
                
                if (input < 1 && str != "x" && str != "y" && str != "Row" && str != "Col")
                {
                    Console.WriteLine($"Number of {str} must be more than 0!");
                    continue;
                }

                if (input >= current && (str == "Col" || str == "Row"))
                {
                    Console.WriteLine("Invalid coordinate!");
                    continue;
                }

                break;
            } while (true);

            return input;
        }

        public static string PrintActionBoard(Game game)
        {
            var sb = new StringBuilder();
            var myBoard = game.PlayerTwoBoard;
            var enemyBoard = game.PlayerOneBoard;
            if (game.PlayerOneTurn)
            {
                myBoard = game.PlayerOneBoard;
                enemyBoard = game.PlayerTwoBoard;
            }
            sb.Append("    ");
            
            for (var g = 0; g < game.Cols; g++)
            {
                sb.Append(" ");
                sb.Append($"{g}".PadRight(3));
            }
            sb.Append(" \t|\t");
            sb.Append("     ");
            
            for (var g = 0; g < game.Cols; g++)
            {
                sb.Append(" ");
                sb.Append($"{g}".PadRight(3));
            }
            
            for (var i = 0; i < game.Rows; i++)
            {
                sb.Append($"\n{i}".PadRight(3));
                sb.Append(" |");
                for (var j = 0; j < game.Cols; j++)
                {
                    switch (myBoard[i][j])
                    {
                        case CellState.Empty:
                            sb.Append(" ~ |");
                            break;
                        case CellState.Miss:
                            sb.Append(" M |");
                            break;
                        case CellState.Hit:
                            sb.Append(" H |");
                            break;
                        case CellState.Ship:
                            sb.Append(" S |");
                            break;
                        case CellState.Wreck:
                            sb.Append(" W |");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                sb.Append("\t|\t");
                sb.Append($"{i}".PadRight(3));
                sb.Append(" |");
                for (var j = 0; j < game.Cols; j++)
                {
                    switch (enemyBoard[i][j])
                    {
                        case CellState.Empty:
                            sb.Append(" ~ |");
                            break;
                        case CellState.Miss:
                            sb.Append(" M |");
                            break;
                        case CellState.Hit:
                            sb.Append(" H |");
                            break;
                        case CellState.Ship:
                            sb.Append(" ~ |");
                            break;
                        case CellState.Wreck:
                            sb.Append(" W |");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return sb.ToString();
        }

        public static void PlayerWon(Game game, DbContext ctx)
        {
            var str = GameState.HasPlayerWon(game);
            if (str != "No")
            {
                Console.Clear();
                Console.WriteLine(PrintActionBoard(game));
                Console.WriteLine($"{str} has won the game");
                game.GameOver = true;
                ctx.SaveChanges();
                WaitForUser();
                Environment.Exit(0);
            }
        }
    }
}