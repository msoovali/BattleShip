using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages
{
    public class ShipPlacement : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public ShipPlacement(DAL.AppDbContext context)
        {
            _context = context;
        }

        public Game Game { get; set; }
        public string Message { get; set; }
        public async Task<IActionResult> OnGet(int id, int? choice)
        {
            Game = await _context.Games.FirstOrDefaultAsync(m => m.GameId == id);
            if (choice == 1)
            {
                if (!BLL.ShipPlacement.RndPlacement(Game, false))
                {
                    Message = "You have set too small board for those ship sizes!";
                    return Page();
                }
                _context.Update(Game);
                _context.SaveChanges();
                return RedirectToPage("PlayGame", new {id = Game.GameId});
            }
            return Page();
        }
    }
}