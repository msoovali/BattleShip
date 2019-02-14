using System;
using System.Threading.Tasks;
using Domain;

namespace BLL
{
    public static class ShipPlacement
    {
        public static bool CanPlaceShip(Game game, Ship ship, int x, int y)
        {
            var board = game.PlayerTwoBoard;
            if (game.PlayerOneTurn)
            {
                board = game.PlayerOneBoard;
            }

            if (ship.ShipLayout == ShipLayout.Vertical)
            {
                for (var i = x; i < x + ship.ShipLength; i++)
                {
                    if (board[i][y] != CellState.Empty)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (var i = y; i < y + ship.ShipLength; i++)
                {
                    if (board[x][i] != CellState.Empty)
                    {
                        return false;
                    }
                }
            }

            if (!game.ShipsCanTouch)
            {
                if (ship.ShipLayout == ShipLayout.Vertical)
                {
                    if (y - 1 >= 0)
                    {
                        for (var i = x; i < x + ship.ShipLength; i++)
                        {
                            if (board[i][y - 1] != CellState.Empty)
                            {
                                return false;
                            }
                        }
                    }

                    if (y + 1 < game.Cols)
                    {
                        for (int i = x; i < x + ship.ShipLength; i++)
                        {
                            if (board[i][y + 1] != CellState.Empty)
                            {
                                return false;
                            }
                        }
                    }

                    if (x - 1 >= 0)
                    {
                        if (board[x - 1][y] != CellState.Empty)
                        {
                            return false;
                        }
                    }

                    if (x + ship.ShipLength < game.Rows)
                    {
                        if (board[x + ship.ShipLength][y] != CellState.Empty)
                        {
                            return false;
                        }
                    }
                }

                else
                {
                    if (x - 1 >= 0)
                    {
                        for (var i = y; i < y + ship.ShipLength; i++)
                        {
                            if (board[x - 1][i] != CellState.Empty)
                            {
                                return false;
                            }
                        }
                    }
                    
                    if (x + 1 < game.Rows)
                    {
                        for (var i = y; i < y + ship.ShipLength; i++)
                        {
                            if (board[x + 1][i] != CellState.Empty)
                            {
                                return false;
                            }
                        }
                    }

                    if (y - 1 >= 0)
                    {
                        if (board[x][y - 1] != CellState.Empty)
                        {
                            return false;
                        }
                    }

                    if (y + ship.ShipLength < game.Cols)
                    {
                        if (board[x][y + ship.ShipLength] != CellState.Empty)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool RndPlacement(Game game, bool randomOnlyAi)
        {
            var task = Task.Run(() => RandomPlacement(game, randomOnlyAi));
            return task.Wait(5000);
        }

        public static void RandomPlacement(Game game, bool randomOnlyAi)
        {
            game.PlayerOneTurn = false;
            RandomHelper(game);
            if (randomOnlyAi) return;
            game.PlayerOneTurn = true;
            RandomHelper(game);
        }

        private static void RandomHelper(Game game)
        {
            var r = new Random();
            foreach (var ship in game.GameShips)
            {
                int x;
                int y;
                do
                {
                    ship.ShipLayout = (ShipLayout) r.Next(0, 2);
                    x = ship.ShipLayout == ShipLayout.Vertical ? r.Next(0, game.Rows - ship.ShipLength) : r.Next(0, game.Rows);
                    
                    y = ship.ShipLayout == ShipLayout.Horizontal ? r.Next(0, game.Cols - ship.ShipLength) : r.Next(0, game.Cols);
                } while (CanPlaceShip(game, ship, x, y) == false);
                InitBoard.PlaceShip(game, ship, x, y);
            }
        }
    }
}