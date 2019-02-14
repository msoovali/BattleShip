using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace BLL
{
    public static class InitBoard
    {
        public static void InitializeBoard(Game game)
        {
            for (var i = 0; i < game.Rows; i++)
            {
                game.PlayerTwoBoard.Add(new List<CellState>());
                game.PlayerOneBoard.Add(new List<CellState>());
                for (var j = 0; j < game.Cols; j++)
                {
                    game.PlayerTwoBoard[i].Add(CellState.Empty);
                    game.PlayerOneBoard[i].Add(CellState.Empty);
                }
            }

        }

        public static void PlaceShip(Game game, Ship ship, int x, int y)
        {
            var board = game.PlayerTwoBoard;
            if (game.PlayerOneTurn)
            {
                board = game.PlayerOneBoard;
            }

            if (ship.ShipLayout == ShipLayout.Vertical)
            {
                for (int i = x; i < x + ship.ShipLength; i++)
                {
                    board[i][y] = CellState.Ship;
                }
            }
            else
            {
                for (int i = y; i < y + ship.ShipLength; i++)
                {
                    board[x][i] = CellState.Ship;
                }
            }
        }

        public static void SetShipSizes(List<int> ships, Game game, DbContext ctx)
        {
            game.GameShips = new List<Ship>();
            for (var i = 0; i < ships.Count; i++)
            {
                game.GameShips.Add(new Ship(ships[i]));
            }

            ctx.Update(game);
            ctx.SaveChanges();
        }
    }
}