using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace WebApp.Pages
{
    public class LoadGame : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public LoadGame(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Game> Game { get;set; }
        public string Search { get; set; }

        public async Task OnGetAsync(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                Game = await _context.Games.ToListAsync();
            }
            else
            {
                search = search.ToLower();
                Game = await _context.Games.Where(g => 
                    g.GameId.ToString() == search ||
                    g.PlayerOne.ToLower().Contains(search) ||
                    g.PlayerTwo.ToLower().Contains(search)).ToListAsync();

                Search = search;
            }
            
        }
    }
}