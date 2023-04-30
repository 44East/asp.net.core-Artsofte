using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Departments
{
    /// <summary>
    /// This is a page model class for deleting a <see cref="Models.Department"/> objects in DB.
    /// </summary>
    public class DeleteModel : PageModel
    {
        private readonly ArtsofteContext _context;
        private readonly ModelsDAL _models;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteModel"/> class.
        /// </summary>
        /// <param name="models">The <see cref="ModelsDAL"/> context.</param>
        public DeleteModel(ModelsDAL models)
        {
            _models = models;
        }

        /// <summary>
        /// Property that binds to the <see cref="Models.Department"/> model.
        /// </summary>
        [BindProperty]
        public Department Department { get; set; } 
        /// <summary>
        /// Shows the current <see cref="Models.Department"/> object before deleting. 
        /// </summary>
        /// <param name="id">This parameter represents the ID of the <see cref="Models.Department"/> object that needs to be deleted..</param>
        /// <returns>The result of the GET request.</returns>
        public IActionResult OnGet(int? id)
        {
            if (id == null || _models.Departments == null)
            {
                return NotFound();
            }

            var department = _models.Departments.FirstOrDefault(m => m.Id == id);

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

        /// <summary>
        /// Handles POST requests for the delete <see cref="Models.Department"/> object.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Models.Department"/> to be deleted.</param>
        /// <returns>The result of the POST request.</returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _models.Departments == null)
            {
                return NotFound();
            }
            var department = _models.Departments.ToList().Find(d => d.Id == id);

            if (department != null)
            {
                Department = department;
                await _models.DeleteDepartmentAsync(department);
            }

            return RedirectToPage("./Index");
        }
    }
}
