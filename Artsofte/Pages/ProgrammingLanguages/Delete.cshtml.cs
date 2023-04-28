using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    /// <summary>
    /// This is a page model class for deleting a <see cref="Models.ProgrammingLanguage"/> objects in DB.
    /// </summary>
    public class DeleteModel : PageModel
    {
        private readonly ArtsofteContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="ArtsofteContext"/> context object.</param>
        public DeleteModel(ArtsofteContext context)
        {
            _context = context;
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
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ProgrammingLanguages == null)
            {
                return NotFound();
            }

            var programminglanguage = await _context.ProgrammingLanguages.FirstOrDefaultAsync(m => m.Id == id);

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
            if (id == null || _context.ProgrammingLanguages == null)
            {
                return NotFound();
            }
            var programminglanguage = await _context.ProgrammingLanguages.FindAsync(id);

            if (programminglanguage != null)
            {
                ProgrammingLanguage = programminglanguage;
                _context.ProgrammingLanguages.Remove(ProgrammingLanguage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
