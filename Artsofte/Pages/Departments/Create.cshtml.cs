using Artsofte.Data;
using Artsofte.Models;
using Artsofte.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artsofte.Pages.Departments
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
        public DepartmentVM DepartmentVM { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Departments == null || DepartmentVM == null)
            {
                return Page();
            }

            var entry = _context.Add(new Department());
            entry.CurrentValues.SetValues(DepartmentVM);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
