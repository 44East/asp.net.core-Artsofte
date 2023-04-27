using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Artsofte.Data;
using Artsofte.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Artsofte.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly Artsofte.Data.ArtsofteContext _context;

        public CreateModel(Artsofte.Data.ArtsofteContext context)
        {
            _context = context;
        }
        /*
        public IActionResult OnGet()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["ProgrammingLanguageId"] = new SelectList(_context.ProgrammingLanguages, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Employee Employee { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            Employee.Department = await _context.Departments.FindAsync(Employee.DepartmentId);
            Employee.ProgrammingLanguage = await _context.ProgrammingLanguages.FindAsync(Employee.ProgrammingLanguageId);

            if (!ModelState.IsValid || _context.Employees == null || Employee == null)
            {
                return Page();
            }

            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        */
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
