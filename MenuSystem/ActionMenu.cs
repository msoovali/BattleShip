using System;
using BLL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace MenuSystem
{
    public static class ActionMenu
    {
        public static void BombingMenu(Game game, DbContext ctx)
        {
            Console.Clear();
            if (!game.PlayerOneTurn && game.Ai)
            {
                Ai.MakeMove(game);
                game.PlayerOneTurn = true;
                ConsoleView.PlayerWon(game, ctx);
                BombingMenu(game, ctx);
            }
            ctx.SaveChanges();
            Console.WriteLine(game.PlayerOneTurn ? $"{game.PlayerOne}'s turn!" : $"{game.PlayerTwo}'s turn!");
            var board = game.PlayerOneBoard;
            if (game.PlayerOneTurn)
            {
                board = game.PlayerTwoBoard;
            }
            ConsoleView.WaitForUser();
            Console.WriteLine($"LEFT one is Your board!\t\t\t\t\tRIGHT one is enemy board, which You are bombing!");
            Console.WriteLine(ConsoleView.PrintActionBoard(game));
            int x;
            int y;
            do
            {
                Console.Write("Enter left coordinate: ");
                x = ConsoleView.IntegerInputHelper("Row", game.Rows);
                Console.Write("Enter top coordinate: ");
                y = ConsoleView.IntegerInputHelper("Col", game.Cols);
                if (board[x][y] == CellState.Miss || board[x][y] == CellState.Hit || board[x][y] == CellState.Wreck)
                {
                    Console.WriteLine("You have already bombed this coordinate!");
                    continue;
                }

                break;
            } while (true);

            if (board[x][y] == CellState.Ship)
            {
                board[x][y] = CellState.Hit;
                GameState.IsShipWrecked(game, x, y);
            }
            else
            {
                board[x][y] = CellState.Miss;
            }

            game.PlayerOneTurn = !game.PlayerOneTurn;
            ConsoleView.PlayerWon(game, ctx);
            ctx.Update(game);
            BombingMenu(game, ctx);
        }
    }
}