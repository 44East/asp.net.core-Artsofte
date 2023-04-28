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
          if (!ModelState.IsValid || _context.ProgrammingLanguages == null || ProgrammingLanguageVM == null)
            {
                return Page();
            }
            //binding data from the ViewModel to the General model and insert it into the DB
            var entry = _context.Add(new ProgrammingLanguage());
            entry.CurrentValues.SetValues(ProgrammingLanguageVM);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
