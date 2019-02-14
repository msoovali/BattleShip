using System;
using System.Collections.Generic;
using Domain;

namespace BLL
{
    public static class GameState
    {
        public static string HasPlayerWon(Game game)
        {
            var playerOneWon = true;
            var playerTwoWon = true;
            for (var i = 0; i < game.Rows; i++)
            {
                for (var j = 0; j < game.Cols; j++)
                {
                    if (game.PlayerOneBoard[i][j] == CellState.Ship)
                    {
                        playerTwoWon = false;
                    }

                    if (game.PlayerTwoBoard[i][j] == CellState.Ship)
                    {
                        playerOneWon = false;
                    }
                    
                }
            }

            if (playerOneWon)
            {
                return game.PlayerOne;
            }

            if (playerTwoWon)
            {
                return game.PlayerTwo;
            }

            return "No";
        }

        public static void IsShipWrecked(Game game, int x, int y)
        {
            var board = game.PlayerOneBoard;
            if (game.PlayerOneTurn)
            {
                board = game.PlayerTwoBoard;
            }
            
            

            var shipWrecked = true;
            for (var i = x; i < game.Rows; i++)
            {
                if (board[i][y] == CellState.Ship)
                {
                    shipWrecked = false;
                }

                if (board[i][y] == CellState.Empty || board[i][y] == CellState.Miss)
                {
                    break;
                }
            }

            for (var i = x; i >= 0; i--)
            {
                if (board[i][y] == CellState.Ship)
                {
                    shipWrecked = false;
                }
                if (board[i][y] == CellState.Empty || board[i][y] == CellState.Miss)
                {
                    break;
                }
            }

            for (var i = y; i < game.Cols; i++)
            {
                if (board[x][i] == CellState.Ship)
                {
                    shipWrecked = false;
                }

                if (board[x][i] == CellState.Empty || board[x][i] == CellState.Miss)
                {
                    break;
                }
            }
            
            for (var i = y; i >= 0; i--)
            {
                if (board[x][i] == CellState.Ship)
                {
                    shipWrecked = false;
                }

                if (board[x][i] == CellState.Empty || board[x][i] == CellState.Miss)
                {
                    break;
                }
            }

            if (shipWrecked)
            {
                for (var i = x; i < game.Rows; i++)
                {
                    if (board[i][y] == CellState.Hit)
                    {
                        board[i][y] = CellState.Wreck;
                    }

                    if (board[i][y] == CellState.Empty || board[i][y] == CellState.Miss)
                    {
                        break;
                    }
                }

                for (var i = x; i >= 0; i--)
                {
                    if (board[i][y] == CellState.Hit)
                    {
                        board[i][y] = CellState.Wreck;
                    }
                    if (board[i][y] == CellState.Empty || board[i][y] == CellState.Miss)
                    {
                        break;
                    }
                }

                for (var i = y; i < game.Cols; i++)
                {
                    if (board[x][i] == CellState.Hit)
                    {
                        board[x][i] = CellState.Wreck;
                    }

                    if (board[x][i] == CellState.Empty || board[x][i] == CellState.Miss)
                    {
                        break;
                    }
                }
            
                for (var i = y; i >= 0; i--)
                {
                    if (board[x][i] == CellState.Hit)
                    {
                        board[x][i] = CellState.Wreck;
                    }

                    if (board[x][i] == CellState.Empty || board[x][i] == CellState.Miss)
                    {
                        break;
                    }
                }
            }
        }
    }
}