using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Employees
{
    /// <summary>
    /// The page model for showing all <see cref="Models.Employee"/> objects.
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
        /// The list of <see cref="Models.Employee"/> to display on the index page.
        /// </summary>
        public IList<Employee> Employee { get;set; } = default!;
        
        /// <summary>
        /// Gets the list of <see cref="Models.Employee"/> from the database.
        /// </summary>
        public async Task OnGetAsync()
        {
            if (_context.Employees != null)
            {
                //Bind data from the other tables
                Employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.ProgrammingLanguage).ToListAsync();
            }
        }
    }
}
