using System.Linq;
using System.Threading.Tasks;
using BLL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class NewGame : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public NewGame(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Game Game { get; set; }
        [BindProperty]
        public bool StandardShips { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            InitBoard.InitializeBoard(Game);
            _context.Games.Add(Game);
            await _context.SaveChangesAsync();
            
            if (!StandardShips)
            {
                return RedirectToPage("CustomShips", new {id = _context.Games.Last().GameId});
            }
            
            return RedirectToPage("ShipPlacement", new {id = _context.Games.Last().GameId});
        }
    }
}