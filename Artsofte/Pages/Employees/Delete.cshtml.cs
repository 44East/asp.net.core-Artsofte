using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Employees
{
    /// <summary>
    /// This is a page model class for deleting a <see cref="Models.Employee"/> objects in DB.
    /// </summary>
    public class DeleteModel : PageModel
    {
        private readonly ModelsDataAccessLayer _models;
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteModel"/> class.
        /// </summary>
        /// <param name="models">The <see cref="ModelsDataAccessLayer"/> context.</param>
        public DeleteModel( ModelsDataAccessLayer models)
        {
            _models = models;
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
        public IActionResult OnGet(int? id)
        {
            if (id == null || _models.Employees == null)
            {
                return NotFound();
            }

            var employee = _models.Employees.FirstOrDefault(m => m.Id == id);

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
            if (id == null || _models.Employees == null)
            {
                return NotFound();
            }
            //Casting an IEnumerable to a List collection and find the necessery Employee
            var employee = _models.Employees.ToList().Find(e => e.Id == id);

            if (employee != null)
            {
                Employee = employee;
                await _models.DeleteEmployeeAsync(employee);
            }

            return RedirectToPage("./Index");
        }
    }
}
