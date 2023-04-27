using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    public class CreateModel : PageModel
    {
        private readonly Artsofte.Data.ArtsofteContext _context;

        public CreateModel(Artsofte.Data.ArtsofteContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProgrammingLanguage ProgrammingLanguage { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.ProgrammingLanguages == null || ProgrammingLanguage == null)
            {
                return Page();
            }

            _context.ProgrammingLanguages.Add(ProgrammingLanguage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
