using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Artsofte.Data;
using Artsofte.Models;
using Artsofte.Models.ViewModels;

namespace Artsofte.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly ArtsofteContext _context;

        public CreateModel(ArtsofteContext context)
        {
            _context = context;
        }
        
        [BindProperty]
        public EmployeeVM EmployeeVM { get; set; }

        public SelectList Departments { get; set; }
        public SelectList ProgrammingLanguages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            
            Departments = new SelectList(_context.Departments, "Id", "Name");
            ProgrammingLanguages = new SelectList(_context.ProgrammingLanguages, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {            
         

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                Departments = new SelectList( _context.Departments, "Id", "Name");
                ProgrammingLanguages = new SelectList(_context.ProgrammingLanguages, "Id", "Name");
                return Page();
            }

            var entry = _context.Add(new Employee());
            entry.CurrentValues.SetValues(EmployeeVM);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
       }
    }
}
