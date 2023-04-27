using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly ArtsofteContext _context;

        public EditModel(ArtsofteContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Employee Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            Employee = employee;
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["ProgrammingLanguageId"] = new SelectList(_context.ProgrammingLanguages, "Id", "Name");
            return Page();
        }

        
        public async Task<IActionResult> OnPostAsync(Employee employee)
        {
            if (employee != null)
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
