using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages
{
    public class CustomPlacement : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CustomPlacement(DAL.AppDbContext context)
        {
            _context = context;
        }

        public Game Game { get; set; }
        public string Message { get; set; }
        public string Player { get; set; }
        public int CurrentShip { get; set; }
        public List<List<CellState>> Board { get; set; }
        public int ShipNo { get; set; }

        public async Task<IActionResult> OnGet(int id, int? shipNo, int? row, int? col, int? vert)
        {
            Game = await _context.Games.FirstOrDefaultAsync(m => m.GameId == id);
            if (shipNo != null && row != null && col != null && vert != null)
            {
                var ship = Game.GameShips[shipNo.Value];
                ShipNo = shipNo.Value;
                if (vert.Value == 1)
                {
                    ship.ShipLayout = ShipLayout.Vertical;
                }

                if (ship.ShipLayout == ShipLayout.Vertical && row.Value + ship.ShipLength <= Game.Rows ||
                    ship.ShipLayout == ShipLayout.Horizontal && col.Value + ship.ShipLength <= Game.Cols)
                {
                    if (BLL.ShipPlacement.CanPlaceShip(Game, ship, row.Value, col.Value))
                    {
                        BLL.InitBoard.PlaceShip(Game, ship, row.Value, col.Value);
                        _context.Update(Game);
                        _context.SaveChanges();
                        ShipNo--;
                        if (ShipNo < 0)
                        {
                            if (Game.PlayerOneTurn)
                            {
                                Game.PlayerOneTurn = false;
                                if (Game.Ai)
                                {
                                    if (!BLL.ShipPlacement.RndPlacement(Game, true))
                                    {
                                        Message = "You have set too small board for those ship sizes!";
                                        return Page();
                                    }

                                    Game.PlayerOneTurn = true;
                                    _context.Update(Game);
                                    _context.SaveChanges();
                                    return RedirectToPage("PlayGame", new {id = Game.GameId});
                                }

                                _context.Update(Game);
                                _context.SaveChanges();
                                return RedirectToPage("CustomPlacement", new {id = Game.GameId});
                            }

                            Game.PlayerOneTurn = true;
                            _context.Update(Game);
                            _context.SaveChanges();
                            return RedirectToPage("PlayGame", new {id = Game.GameId});
                        }
                    }
                    else
                    {
                        Message = "Other ship at coordinates";
                    }
                }
                else
                {
                    var layOut = vert == 1 ? "vertically" : "horizontally";
                    Message = $"Ship cannot be placed on coordinates ({row}, {col}) {layOut}";
                }
            }
            else
            {
                ShipNo = Game.NumberOfShips - 1;
            }

            if (ShipNo >= 0)
            {
                CurrentShip = Game.GameShips[ShipNo].ShipLength;
            }

            if (Game.PlayerOneTurn)
            {
                Player = Game.PlayerOne;
                Board = Game.PlayerOneBoard;
            }
            else
            {
                Player = Game.PlayerTwo;
                Board = Game.PlayerTwoBoard;
            }

            return Page();
        }
    }
}