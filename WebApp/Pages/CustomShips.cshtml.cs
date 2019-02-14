using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages
{
    public class CustomShips : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CustomShips(DAL.AppDbContext context)
        {
            _context = context;
        }
        
        [BindProperty] public int GameId { get; set; }
        [BindProperty] public List<int> ShipLengths { get; set; } = new List<int>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(m => m.GameId == id);
            
            if (game == null)
            {
                return NotFound();
            }

            GameId = game.GameId;
            for (var i = 0; i < game.NumberOfShips; i++)
            {
                ShipLengths.Add(1);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var game = await _context.Games.FirstOrDefaultAsync(m => m.GameId == GameId);

            BLL.InitBoard.SetShipSizes(ShipLengths, game, _context);

            return RedirectToPage("ShipPlacement", new {id = game.GameId});
        }
    }
}