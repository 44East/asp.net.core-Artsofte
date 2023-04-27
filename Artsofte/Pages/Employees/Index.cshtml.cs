using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly ArtsofteContext _context;

        public IndexModel(ArtsofteContext context)
        {
            _context = context;
        }

        public IList<Employee> Employee { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Employees != null)
            {
                Employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.ProgrammingLanguage).ToListAsync();
            }
        }
    }
}
