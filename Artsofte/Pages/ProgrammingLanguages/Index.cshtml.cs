using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    public class IndexModel : PageModel
    {
        private readonly ArtsofteContext _context;

        public IndexModel(ArtsofteContext context)
        {
            _context = context;
        }

        public IList<ProgrammingLanguage> ProgrammingLanguage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.ProgrammingLanguages != null)
            {
                ProgrammingLanguage = await _context.ProgrammingLanguages.ToListAsync();
            }
        }
    }
}
