using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    /// <summary>
    /// Page model for displaying the details of the selected <see cref="Models.ProgrammingLanguage"/> object.
    /// </summary>
    public class DetailsModel : PageModel
    {
        private readonly ModelsDAL _models;
        /// <summary>
        /// Creates a new instance of the <see cref="DetailsModel"/> class.
        /// </summary>
        /// <param name="models">The database context <see cref="ModelsDAL"/>  for this page.</param>
        public DetailsModel(ModelsDAL models)
        {
            _models = models;
        }
        /// <summary>
        /// Property that binds to the <see cref="Models.ProgrammingLanguage"/> model.
        /// </summary>
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
        
        /// <summary>
        /// Handles the HTTP GET request for displaying the details of a current <see cref="Models.ProgrammingLanguage"/> object.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Models.ProgrammingLanguage"/> to display.</param>
        /// <returns>The result of the HTTP GET request.</returns>
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
    }
}
