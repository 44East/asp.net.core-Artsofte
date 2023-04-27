using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    public class DeleteModel : PageModel
    {
        private readonly ArtsofteContext _context;

        public DeleteModel(ArtsofteContext context)
        {
            _context = context;
        }

        [BindProperty]
      public ProgrammingLanguage ProgrammingLanguage { get; set; } = default!;

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
