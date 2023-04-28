using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    /// <summary>
    /// The PageModel class for editing a selected <see cref="Models.ProgrammingLanguage"/> object.
    /// </summary>
    public class EditModel : PageModel
    {
        private readonly ArtsofteContext _context;
        
        /// <summary>
        /// Creates a new instance of the <see cref="EditModel"/> class.
        /// </summary>
        /// <param name="context">The database context <see cref="ArtsofteContext"/>  for this page.</param>
        public EditModel(ArtsofteContext context)
        {
            _context = context;
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
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ProgrammingLanguages == null)
            {
                return NotFound();
            }

            var programminglanguage =  await _context.ProgrammingLanguages.FirstOrDefaultAsync(m => m.Id == id);
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
                _context.ProgrammingLanguages.Update(programmingLanguage);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }

    }
}
