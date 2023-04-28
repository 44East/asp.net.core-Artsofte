using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;
using Artsofte.Models.SortingStates;

namespace Artsofte.Pages.Employees
{
    /// <summary>
    /// The page model for showing all <see cref="Models.Employee"/> objects.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ArtsofteContext _context;

        /// <summary>
        /// Creates a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="context">The database context <see cref="ArtsofteContext"/>  for this page.</param>
        public IndexModel(ArtsofteContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The list of <see cref="Models.Employee"/> to display on the index page.
        /// </summary>
        public IList<Employee> Employees { get; set; }

        /// <summary>
        /// Retrieves a list of <see cref="Models.Employee"/> from the database and sorts them based on the specified <see cref="EmployeeSortState"/> states.
        /// </summary>
        /// <param name="sortOrder">The sorting order for the employee data. Default value is <see cref="EmployeeSortState.SurnameAsc"/>.</param>
        /// <returns>A task that represents the asynchronous operation of retrieving and sorting employees.</returns>
        public async Task OnGetAsync(EmployeeSortState sortOrder = EmployeeSortState.SurnameAsc)
        {
            if (_context.Employees == null)
            {
                // If there are no Employees in the database, initialize an empty list and return.
                Employees = new List<Employee>();
                return;
            }
            // Create an IQueryable object for the Employees in the database, including related entities.
            IQueryable<Employee> employeesIQ = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.ProgrammingLanguage);

            // Set up ViewData for the sort order. These values will be used in the Razor view to create links to sort the data.
            ViewData["NameSort"] = sortOrder == EmployeeSortState.NameAsc ? EmployeeSortState.NameDesc : EmployeeSortState.NameAsc;
            ViewData["SurnameSort"] = sortOrder == EmployeeSortState.SurnameAsc ? EmployeeSortState.SurnameDesc : EmployeeSortState.SurnameAsc;
            ViewData["AgeSort"] = sortOrder == EmployeeSortState.AgeAsc ? EmployeeSortState.AgeDesc : EmployeeSortState.AgeAsc;
            ViewData["GenderSort"] = sortOrder == EmployeeSortState.GenderAsc ? EmployeeSortState.GenderDesc : EmployeeSortState.GenderAsc;
            ViewData["DepartmentSort"] = sortOrder == EmployeeSortState.DepartmentAsc ? EmployeeSortState.DepartmentDesc : EmployeeSortState.DepartmentAsc;
            ViewData["ProgrammingLanguageSort"] = sortOrder == EmployeeSortState.ProgLangAsc ? EmployeeSortState.ProgLangDesc : EmployeeSortState.ProgLangAsc;

            // Sort the Employees based on the sortOrder parameter by EmployeeSortState.
            // The switch statement selects the appropriate LINQ method based on the sortOrder value.
            // If an invalid value is passed, the default case sorts by Surname.
            employeesIQ = sortOrder switch
            {
                EmployeeSortState.NameAsc => employeesIQ.OrderBy(e => e.Name),
                EmployeeSortState.NameDesc => employeesIQ.OrderByDescending(e => e.Name),
                EmployeeSortState.SurnameDesc => employeesIQ.OrderByDescending(e => e.Surname),
                EmployeeSortState.AgeAsc => employeesIQ.OrderBy(e => e.Age),
                EmployeeSortState.AgeDesc => employeesIQ?.OrderByDescending(e => e.Age),
                EmployeeSortState.GenderAsc => employeesIQ.OrderBy(e => e.Gender),
                EmployeeSortState.GenderDesc => employeesIQ.OrderByDescending(e => e.Gender),
                EmployeeSortState.DepartmentAsc => employeesIQ.OrderBy(e => e.Department.Name),
                EmployeeSortState.DepartmentDesc => employeesIQ.OrderByDescending(e => e.Department.Name),
                EmployeeSortState.ProgLangAsc => employeesIQ.OrderBy(e => e.ProgrammingLanguage.Name),
                EmployeeSortState.ProgLangDesc => employeesIQ.OrderByDescending(e => e.ProgrammingLanguage.Name),
                _ => employeesIQ.OrderBy(e => e.Surname)
            };

            // Bind the sorted data to the Employees property, with AsNoTracking() to improve performance.
            Employees = await employeesIQ.AsNoTracking().ToListAsync();


        }
    }
}
