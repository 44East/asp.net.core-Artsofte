using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Artsofte.Data;
using Artsofte.Models;
using Artsofte.Models.ViewModels;

namespace Artsofte.Pages.ProgrammingLanguages
{
    /// <summary>
    /// The page for creating a new <see cref="Models.ProgrammingLanguage"/> object.
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
        /// The page handler for displaying the form to create a new <see cref="Models.ProgrammingLanguage"/> object.
        /// </summary>
        /// <returns>The result of the form display.</returns>
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// View model <see cref="Models.ProgrammingLanguage"/> for binding data from Creation page fields
        /// </summary>
        [BindProperty]
        public ProgrammingLanguageVM ProgrammingLanguageVM { get; set; } = default!;

        /// <summary>
        /// This method handles the submission of a form for adding a new <see cref="ProgrammingLanguage"/> object using the data provided in the 
        /// <see cref="Models.ViewModels.ProgrammingLanguageVM"/> view model.
        /// </summary>
        /// <returns>The result of the form submission.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid ||  ProgrammingLanguageVM == null)
            {
                return Page();
            }
            //binding data on the ViewModel and insert it into the DB
            await _models.InsertProgrammingLanguages(ProgrammingLanguageVM);

            return RedirectToPage("./Index");
        }
    }
}
