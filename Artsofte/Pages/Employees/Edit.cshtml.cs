using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Employees
{
    /// <summary>
    /// The PageModel class for editing a selected <see cref="Models.Employee"/> object.
    /// </summary>
    public class EditModel : PageModel
    {
        private readonly ArtsofteContext _context;
        /// <summary>
        /// Creates a new instance of the <see cref="EditModel"/> class.
        /// </summary>
        /// <param name="context">The database context <see cref="ArtsofteContext"/>  for this page.</param>
        public EditModel(ArtsofteContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Property that binds to the <see cref="Models.Employee"/> model.
        /// </summary>
        [BindProperty]
        public Employee Employee { get; set; } = default!;
        
        /// <summary>
        /// Handles the GET request for editing a selected <see cref="Models.Employee"/> object.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Models.Employee"/> to edit.</param>
        /// <returns>The result of the GET request.</returns>
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

        /// <summary>
        /// Handles the POST request for editing a selected <see cref="Models.Employee"/> object.
        /// </summary>
        /// <param name="employee">The updated <see cref="Models.Employee"/> object to save.</param>
        /// <returns>The result of the POST request.</returns>
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
    }
}
