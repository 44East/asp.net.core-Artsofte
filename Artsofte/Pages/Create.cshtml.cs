using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages
{
    public class CreateModel : PageModel
    {
        private readonly Artsofte.Data.ArtsofteContext _context;

        public CreateModel(Artsofte.Data.ArtsofteContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["DepartmentId"] = new SelectList(_context.Set<Department>(), "Id", "Id");
        ViewData["ProgrammingLanguageId"] = new SelectList(_context.Set<ProgrammingLanguage>(), "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Employee Employee { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Employees == null || Employee == null)
            {
                return Page();
            }

            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
