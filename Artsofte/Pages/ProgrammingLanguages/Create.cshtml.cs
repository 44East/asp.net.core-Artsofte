using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Artsofte.Data;
using Artsofte.Models;
using Artsofte.Models.ViewModels;

namespace Artsofte.Pages.ProgrammingLanguages
{
    public class CreateModel : PageModel
    {
        private readonly ArtsofteContext _context;

        public CreateModel(ArtsofteContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProgrammingLanguageVM ProgrammingLanguageVM { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.ProgrammingLanguages == null || ProgrammingLanguageVM == null)
            {
                return Page();
            }

            var entry = _context.Add(new ProgrammingLanguage());
            entry.CurrentValues.SetValues(ProgrammingLanguageVM);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
