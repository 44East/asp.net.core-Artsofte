using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly Artsofte.Data.ArtsofteContext _context;

        public DetailsModel(Artsofte.Data.ArtsofteContext context)
        {
            _context = context;
        }

        public Employee Employee { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                                  .Include(d => d.Department)
                                  .Include(p => p.ProgrammingLanguage)
                                  .FirstOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            else 
            {
                Employee = employee;
            }
            return Page();
        }
    }
}
