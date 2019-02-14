using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages
{
    public class PlayGame : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public PlayGame(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; }
        [BindProperty]
        public string Message { get; set; }
        [BindProperty]
        public List<List<CellState>> MyBoard { get; set; }
        [BindProperty]
        public List<List<CellState>> EnemyBoard { get; set; }
        
        [BindProperty] public string Me { get; set; }
        [BindProperty] public string Enemy { get; set; }

        public async Task<IActionResult> OnGet(int id, int? row, int? col)
        {
            Game = await _context.Games.FirstOrDefaultAsync(m => m.GameId == id);
            Me = Game.PlayerTwo;
            Enemy = Game.PlayerOne;
            EnemyBoard = Game.PlayerOneBoard;
            MyBoard = Game.PlayerTwoBoard;
            if (Game.PlayerOneTurn)
            {
                Me = Game.PlayerOne;
                Enemy = Game.PlayerTwo;
                EnemyBoard = Game.PlayerTwoBoard;
                MyBoard = Game.PlayerOneBoard;
            }
            if (row != null && col!= null)
            {
                switch (EnemyBoard[row.Value][col.Value])
                {
                    case CellState.Hit:
                        Message = "You have already bombed this coordinate!";
                        break;
                    case CellState.Empty:
                        EnemyBoard[row.Value][col.Value] = CellState.Miss;
                        if (Game.Ai)
                        {
                            BLL.Ai.MakeMove(Game);
                        }
                        else
                        {
                            Game.PlayerOneTurn = !Game.PlayerOneTurn;
                        }
                        break;
                    case CellState.Miss:
                        Message = "You have already bombed this coordinate!";
                        break;
                    case CellState.Ship:
                        EnemyBoard[row.Value][col.Value] = CellState.Hit;
                        BLL.GameState.IsShipWrecked(Game, row.Value, col.Value);
                        if (Game.Ai)
                        {
                            BLL.Ai.MakeMove(Game);
                        }
                        else
                        {
                            Game.PlayerOneTurn = !Game.PlayerOneTurn;
                        }
                        var msg = BLL.GameState.HasPlayerWon(Game);
                        if (msg != "No")
                        {
                            Message = $"{msg} has won the game!";
                            Game.GameOver = true;
                        }
                        break;
                    case CellState.Wreck:
                        Message = "You have already bombed this coordinate!";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Me = Game.PlayerTwo;
                Enemy = Game.PlayerOne;
                EnemyBoard = Game.PlayerOneBoard;
                MyBoard = Game.PlayerTwoBoard;
                if (Game.PlayerOneTurn)
                {
                    Me = Game.PlayerOne;
                    Enemy = Game.PlayerTwo;
                    EnemyBoard = Game.PlayerTwoBoard;
                    MyBoard = Game.PlayerOneBoard;
                }
                _context.Update(Game);
                _context.SaveChanges();
            }
            
            if (Game == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}