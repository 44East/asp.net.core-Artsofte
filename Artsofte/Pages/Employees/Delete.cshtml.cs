using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Employees
{
    /// <summary>
    /// This is a page model class for deleting a <see cref="Models.Employee"/> objects in DB.
    /// </summary>
    public class DeleteModel : PageModel
    {
        private readonly ArtsofteContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="ArtsofteContext"/> context.</param>
        public DeleteModel(ArtsofteContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Property that binds to the <see cref="Models.Employee"/> model.
        /// </summary>
        [BindProperty]
        public Employee Employee { get; set; } = default!;

        /// <summary>
        /// Shows the current <see cref="Models.Employee"/> object before deleting. 
        /// </summary>
        /// <param name="id">This parameter represents the ID of the <see cref="Models.Employee"/> object that needs to be deleted..</param>
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
            else
            {
                Employee = employee;
            }
            return Page();
        }

        /// <summary>
        /// Handles POST requests for the delete <see cref="Models.ProgrammingLanguage"/> object.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Models.ProgrammingLanguage"/> to be deleted.</param>
        /// <returns>The result of the POST request.</returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                Employee = employee;
                _context.Employees.Remove(Employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
