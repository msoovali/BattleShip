using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Game
    {
        public int GameId { get; set; }

        [MaxLength(64), MinLength(1), Required] public string PlayerOne { get; set; }
        [MaxLength(64), MinLength(1), Required] public string PlayerTwo { get; set; } = "AI";

        public List<List<CellState>> PlayerOneBoard { get; set; } = new List<List<CellState>>();

        public List<List<CellState>> PlayerTwoBoard { get; set; } = new List<List<CellState>>();
        
        public List<Ship> GameShips { get; set; } = new List<Ship>
        {
            new Ship(5),
            new Ship(4),
            new Ship(3),
            new Ship(2),
            new Ship(1)
        };
        
        [Range(1,99)]
        public int Rows { get; set; } = 10;
        [Range(1,99)]
        public int Cols { get; set; } = 10;

        public bool PlayerOneTurn { get; set; } = true;

        public bool GameOver { get; set; } = false;
        [Range(1, 99)]
        public int NumberOfShips { get; set; } = 5;
        public bool Ai { get; set; } = true;
        public bool ShipsCanTouch { get; set; } = false;
    }
}