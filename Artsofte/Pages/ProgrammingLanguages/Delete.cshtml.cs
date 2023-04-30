using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    /// <summary>
    /// This is a page model class for deleting a <see cref="Models.ProgrammingLanguage"/> objects in DB.
    /// </summary>
    public class DeleteModel : PageModel
    {
        private readonly ModelsDataAccessLayer _models;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteModel"/> class.
        /// </summary>
        /// <param name="models">The <see cref="ModelsDataAccessLayer"/> context object.</param>
        public DeleteModel(ModelsDataAccessLayer models)
        {
            _models = models;
        }

        /// <summary>
        /// Property that binds to the <see cref="Models.ProgrammingLanguage"/> model.
        /// </summary>
        [BindProperty]
        public ProgrammingLanguage ProgrammingLanguage { get; set; } = default!;
        
        /// <summary>
        /// Shows the current <see cref="Models.ProgrammingLanguage"/> object before deleting. 
        /// </summary>
        /// <param name="id">This parameter represents the ID of the <see cref="Models.ProgrammingLanguage"/> object that needs to be deleted..</param>
        /// <returns>The result of the GET request.</returns>
        public IActionResult OnGet(int? id)
        {
            if (id == null || _models.ProgrammingLanguages == null)
            {
                return NotFound();
            }

            var programminglanguage = _models.ProgrammingLanguages.FirstOrDefault(m => m.Id == id);

            if (programminglanguage == null)
            {
                return NotFound();
            }
            else
            {
                ProgrammingLanguage = programminglanguage;
            }
            return Page();
        }
        /// <summary>
        /// Handles POST requests for the delete <see cref="Models.ProgrammingLanguage"/> object.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Models.ProgrammingLanguage"/> to be deleted.</param>
        /// <returns>The result of the POST request.</returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _models.ProgrammingLanguages == null)
            {
                return NotFound();
            }
            var programminglanguage = _models.ProgrammingLanguages.ToList().Find(p => p.Id == id);

            if (programminglanguage != null)
            {
                ProgrammingLanguage = programminglanguage;
                await _models.DeleteProgrammingLanguageAsync(programminglanguage);
            }

            return RedirectToPage("./Index");
        }
    }
}
