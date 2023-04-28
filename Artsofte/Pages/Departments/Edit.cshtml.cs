using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Departments
{
    /// <summary>
    /// The PageModel class for editing a selected <see cref="Models.Department"/> object.
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
        /// Property that binds to the <see cref="Models.Department"/> model.
        /// </summary>
        [BindProperty]
        public Department Department { get; set; } = default!;
        /// <summary>
        /// Handles the GET request for editing a selected <see cref="Models.Department"/> object.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Models.Department"/> to edit.</param>
        /// <returns>The result of the GET request.</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department =  await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            Department = department;
            return Page();
        }

        /// <summary>
        /// Handles the POST request for editing a selected <see cref="Models.Department"/> object.
        /// </summary>
        /// <param name="department">The updated <see cref="Models.Department"/> object to save.</param>
        /// <returns>The result of the POST request.</returns>
        public async Task<IActionResult> OnPostAsync(Department department)
        {
            if (department != null)
            {
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }

    }
}
