using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Employees
{
    /// <summary>
    /// Page model for displaying the details of the selected <see cref="Models.Employee"/> object.
    /// </summary>
    public class DetailsModel : PageModel
    {
        private readonly ArtsofteContext _context;
        /// <summary>
        /// Creates a new instance of the <see cref="DetailsModel"/> class.
        /// </summary>
        /// <param name="context">The database context <see cref="ArtsofteContext"/>  for this page.</param>
        public DetailsModel(ArtsofteContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Property that binds to the <see cref="Models.Employee"/> model.
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Handles the HTTP GET request for displaying the details of a current <see cref="Models.Employee"/> object.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Models.Employee"/> to display.</param>
        /// <returns>The result of the HTTP GET request.</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }
            //Binding data from the conected tables
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
