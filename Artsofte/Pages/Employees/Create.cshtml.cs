using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Artsofte.Data;
using Artsofte.Models;
using Artsofte.Models.ViewModels;

namespace Artsofte.Pages.Employees
{
    /// <summary>
    /// The page for creating a new <see cref="Models.Employee"/> object.
    /// </summary>
    public class CreateModel : PageModel
    {
        private readonly ModelsDAL _models;
        /// <summary>
        /// Creates a new instance of the <see cref="CreateModel"/> class.
        /// </summary>
        /// <param name="models">The database context <see cref="ModelsDAL"/>  for this page.</param>
        public CreateModel(ModelsDAL models)
        {
            _models = models;
        }

        /// <summary>
        /// View model <see cref="Models.Employee"/> for binding data from Creation page fields
        /// </summary>
        [BindProperty]
        public EmployeeVM EmployeeVM { get; set; }

        /// <summary>
        /// Selection List of the current objects from <see cref="Models.Department"/> table.
        /// </summary>
        public SelectList Departments { get; set; }
        /// <summary>
        /// Selection List of the current objects from <see cref="Models.ProgrammingLanguage"/> table.
        /// </summary>
        public SelectList ProgrammingLanguages { get; set; }

        /// <summary>
        /// The page handler for displaying the form to create a new <see cref="Models.Employee"/> object.
        /// </summary>        
        public IActionResult OnGet()
        {
            //Add data from other tables into selection menus.
            Departments = new SelectList(_models.Departments, "Id", "Name");
            ProgrammingLanguages = new SelectList(_models.ProgrammingLanguages, "Id", "Name");
            return Page();
        }
        /// <summary>
        /// This method handles the submission of a form for adding a new <see cref="Models.Employee"/> object using the data provided in the 
        /// <see cref="Models.ViewModels.EmployeeVM"/> view model.
        /// </summary>
        /// <returns>The result of the form submission.</returns>
        public async Task<IActionResult> OnPostAsync()
        {


            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    await Console.Out.WriteLineAsync(error.ErrorMessage);
                }
                //Update info into selection menus after refresh the page
                Departments = new SelectList(_models.Departments, "Id", "Name");
                ProgrammingLanguages = new SelectList(_models.ProgrammingLanguages, "Id", "Name");
                return Page();
            }
            //binding data on the ViewModel and insert it into the DB            
            await _models.InsertEmployee(EmployeeVM);

            return RedirectToPage("./Index");
        }
    }
}
