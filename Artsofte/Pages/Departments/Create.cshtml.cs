using Artsofte.Data;
using Artsofte.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artsofte.Pages.Departments
{
    /// <summary>
    /// The page for creating a new <see cref="Models.Department"/> object.
    /// </summary>
    public class CreateModel : PageModel
    {
        private readonly ModelsDataAccessLayer _models;

        /// <summary>
        /// Creates a new instance of the <see cref="CreateModel"/> class.
        /// </summary>
        /// <param name="models">The database context <see cref="ModelsDataAccessLayer"/>  for this page.</param>
        public CreateModel(ModelsDataAccessLayer models)
        {
            _models = models;
        }

        /// <summary>
        /// The page handler for displaying the form to create a new <see cref="Models.Department"/> object.
        /// </summary>
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// View model <see cref="Models.Department"/> for binding data from Creation page fields
        /// </summary>
        [BindProperty]
        public DepartmentVM DepartmentVM { get; set; } = default!;


        /// <summary>
        /// This method handles the submission of a form for adding a new <see cref="Models.Department"/> object using the data provided in the 
        /// <see cref="Models.ViewModels.DepartmentVM"/> view model.
        /// </summary>
        /// <returns>The result of the form submission.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // send data of the ViewModel and insert it into the DB.
            await _models.InsertDepartmentAsync(DepartmentVM);

            return RedirectToPage("./Index");
        }
    }
}
