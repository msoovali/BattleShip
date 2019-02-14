using System;
using Domain;

namespace BLL
{
    public static class Ai
    {
        public static void MakeMove(Game game)
        {
            var r = new Random();
            int x;
            int y;
            do
            {
                x = r.Next(0, game.Rows);
                y = r.Next(0, game.Cols);
                if (game.PlayerOneBoard[x][y] == CellState.Hit || game.PlayerOneBoard[x][y] == CellState.Miss)
                {
                    continue;
                }
                break;
            } while (true);
            if (game.PlayerOneBoard[x][y] == CellState.Ship)
            {
                game.PlayerOneBoard[x][y] = CellState.Hit;
                GameState.IsShipWrecked(game, x, y);
            }
            else
            {
                game.PlayerOneBoard[x][y] = CellState.Miss;
            }
            game.PlayerOneTurn = true;
        }
    }
}