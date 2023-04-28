using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    /// <summary>
    /// The page model for showing all <see cref="Models.ProgrammingLanguage"/> objects.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ArtsofteContext _context;
        
        /// <summary>
        /// Creates a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="context">The database context <see cref="ArtsofteContext"/>  for this page.</param>
        public IndexModel(ArtsofteContext context)
        {
            _context = context;
        }
        /// <summary>
        /// The list of <see cref="Models.ProgrammingLanguage"/> to display on the index page.
        /// </summary>
        public IList<ProgrammingLanguage> ProgrammingLanguage { get;set; } = default!;

        /// <summary>
        /// Gets the list of <see cref="Models.ProgrammingLanguage"/> from the database.
        /// </summary>
        public async Task OnGetAsync()
        {
            if (_context.ProgrammingLanguages != null)
            {
                ProgrammingLanguage = await _context.ProgrammingLanguages.ToListAsync();
            }
        }
    }
}
