using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.Departments
{
    /// <summary>
    /// Page model for displaying the details of the selected <see cref="Models.Department"/> object.
    /// </summary>
    public class DetailsModel : PageModel
    {
        private readonly ModelsDataAccessLayer _models;
        /// <summary>
        /// Creates a new instance of the <see cref="DetailsModel"/> class.
        /// </summary>
        /// <param name="context">The database context <see cref="ModelsDataAccessLayer"/>  for this page.</param>
        public DetailsModel(ModelsDataAccessLayer models)
        {
            _models = models;

        }
        /// <summary>
        /// Property that binds to the <see cref="Models.Department"/> model.
        /// </summary>
        public Department Department { get; set; }
        
        /// <summary>
        /// Handles the HTTP GET request for displaying the details of a current <see cref="Models.Department"/> object.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Models.Department"/> to display.</param>
        /// <returns>The result of the HTTP GET request.</returns>
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
    }
}
