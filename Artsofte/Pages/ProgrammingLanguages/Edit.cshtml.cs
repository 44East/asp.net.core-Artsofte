using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    public class EditModel : PageModel
    {
        private readonly ArtsofteContext _context;

        public EditModel(ArtsofteContext context)
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

            var programminglanguage =  await _context.ProgrammingLanguages.FirstOrDefaultAsync(m => m.Id == id);
            if (programminglanguage == null)
            {
                return NotFound();
            }
            ProgrammingLanguage = programminglanguage;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
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
