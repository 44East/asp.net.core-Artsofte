using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Departments
{
    /// <summary>
    /// The PageModel class for editing a selected <see cref="Models.Department"/> object.
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
        /// Property that binds to the <see cref="Models.Department"/> model.
        /// </summary>
        [BindProperty]
        public Department Department { get; set; } 
        /// <summary>
        /// Handles the GET request for editing a selected <see cref="Models.Department"/> object.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Models.Department"/> to edit.</param>
        /// <returns>The result of the GET request.</returns>
        public IActionResult OnGet(int? id)
        {
            if (id == null || _models.Departments == null)
            {
                return NotFound();
            }

            var department =  _models.Departments.FirstOrDefault(m => m.Id == id);
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
                await _models.UpdateDepartmentAsync(department);
                return RedirectToPage("./Index");
            }
            return Page();
        }

    }
}
