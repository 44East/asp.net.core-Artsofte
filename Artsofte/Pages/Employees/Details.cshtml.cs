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
        private readonly ModelsDataAccessLayer _models;
        /// <summary>
        /// Creates a new instance of the <see cref="DetailsModel"/> class.
        /// </summary>
        /// <param name="models">The database context <see cref="ModelsDataAccessLayer"/>  for this page.</param>
        public DetailsModel(ModelsDataAccessLayer models)
        {
            _models = models;
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
        public IActionResult OnGet(int? id)
        {
            if (id == null || _models.Employees == null)
            {
                return NotFound();
            }
            //Binding data from the conected tables
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
    }
}
