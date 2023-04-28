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
        private readonly ArtsofteContext _context;

        /// <summary>
        /// Creates a new instance of the <see cref="CreateModel"/> class.
        /// </summary>
        /// <param name="context">The database context <see cref="ArtsofteContext"/>  for this page.</param>
        public CreateModel(ArtsofteContext context)
        {
            _context = context;
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
        public IActionResult OnGetAsync()
        {
            //Add data from other tables into selection menus.
            Departments = new SelectList(_context.Departments, "Id", "Name");
            ProgrammingLanguages = new SelectList(_context.ProgrammingLanguages, "Id", "Name");
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
                    Console.WriteLine(error.ErrorMessage);
                }
                //Update info into selection menus after refresh the page
                Departments = new SelectList(_context.Departments, "Id", "Name");
                ProgrammingLanguages = new SelectList(_context.ProgrammingLanguages, "Id", "Name");
                return Page();
            }
            //binding data from the ViewModel to the General model and insert it into the DB
            var entry = _context.Add(new Employee());
            entry.CurrentValues.SetValues(EmployeeVM);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
