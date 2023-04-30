using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    /// <summary>
    /// The PageModel class for editing a selected <see cref="Models.ProgrammingLanguage"/> object.
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
        /// Property that binds to the <see cref="Models.ProgrammingLanguage"/> model.
        /// </summary>
        [BindProperty]
        public ProgrammingLanguage ProgrammingLanguage { get; set; } = default!;
        
        /// <summary>
        /// Handles the GET request for editing a selected <see cref="Models.ProgrammingLanguage"/> object.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Models.ProgrammingLanguage"/> to edit.</param>
        /// <returns>The result of the GET request.</returns>
        public IActionResult OnGet(int? id)
        {
            if (id == null || _models.ProgrammingLanguages == null)
            {
                return NotFound();
            }

            var programminglanguage =  _models.ProgrammingLanguages.FirstOrDefault(m => m.Id == id);
            if (programminglanguage == null)
            {
                return NotFound();
            }
            ProgrammingLanguage = programminglanguage;
            return Page();
        }
        /// <summary>
        /// Handles the POST request for editing a selected <see cref="Models.ProgrammingLanguage"/> object.
        /// </summary>
        /// <param name="programmingLanguage">The updated <see cref="Models.ProgrammingLanguage"/> object to save.</param>
        /// <returns>The result of the POST request.</returns>
        public async Task<IActionResult> OnPostAsync(ProgrammingLanguage programmingLanguage)
        {
            if (programmingLanguage != null)
            {
                await _models.UpdateProgrammingLanguagesAsync(programmingLanguage);
                return RedirectToPage("./Index");
            }
            return Page();
        }

    }
}
