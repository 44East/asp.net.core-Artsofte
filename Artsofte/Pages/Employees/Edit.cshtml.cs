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
        private readonly ModelsDataAccessLayer _models;
        /// <summary>
        /// Creates a new instance of the <see cref="EditModel"/> class.
        /// </summary>
        /// <param name="models">The database context <see cref="ModelsDataAccessLayer"/>  for this page.</param>
        public EditModel(ModelsDataAccessLayer models)
        {
            _models = models;
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
        public  IActionResult OnGet(int? id)
        {
            if (id == null || !_models.IsDBExist)
            {
                return NotFound();
            }

            var employee =  _models.Employees.FirstOrDefault(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            Employee = employee;
            ViewData["DepartmentId"] = new SelectList(_models.Departments, "Id", "Name");
            ViewData["ProgrammingLanguageId"] = new SelectList(_models.ProgrammingLanguages, "Id", "Name");
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
                await _models.UpdateEmployeeAsync(employee);                
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
