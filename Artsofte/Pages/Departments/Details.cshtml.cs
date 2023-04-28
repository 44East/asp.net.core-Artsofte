using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Departments
{
    /// <summary>
    /// Page model for displaying the details of the selected <see cref="Models.Department"/> object.
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
        /// Property that binds to the <see cref="Models.Department"/> model.
        /// </summary>
        public Department Department { get; set; } = default!;
        
        /// <summary>
        /// Handles the HTTP GET request for displaying the details of a current <see cref="Models.Department"/> object.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Models.Department"/> to display.</param>
        /// <returns>The result of the HTTP GET request.</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            else
            {
                Department = department;
            }
            return Page();
        }
    }
}
