using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;
using Artsofte.Models.SortingStates;

namespace Artsofte.Pages.Departments
{
    /// <summary>
    /// The page model for showing all <see cref="Models.Department"/> objects.
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
        /// The list of <see cref="Models.Department"/> to display on the index page.
        /// </summary>
        public IList<Department> Departments { get; set; }

        /// <summary>
        /// Retrieves a list of <see cref="Models.Department"/> from the database and sorts them based on the specified <see cref="DepartmentSortState"/> states.
        /// </summary>
        /// <param name="sortOrder">The sorting order for the employee data. Default value is <see cref="DepartmentSortState.NameAsc"/>.</param>
        /// <returns>A task that represents the asynchronous operation of retrieving and sorting employees.</returns>
        public async Task OnGetAsync(DepartmentSortState sortOrder = DepartmentSortState.NameAsc)
        {
            if (_context.Departments == null)
            {
                // If there are no Departments in the database, initialize an empty list and return.
                Departments = new List<Department>();
                return;
            }
            // Create an IQueryable object for the Departments in the database, including related entities.
            IQueryable<Department> departmentsIQ = _context.Departments;

            // Set up ViewData for the sort order. These values will be used in the Razor view to create links to sort the data.
            ViewData["NameSort"] = sortOrder == DepartmentSortState.NameAsc ? DepartmentSortState.NameDesc : DepartmentSortState.NameAsc;
            ViewData["FloorSort"] = sortOrder == DepartmentSortState.FloorAsc ? DepartmentSortState.FloorDesc : DepartmentSortState.FloorAsc;

            // Sort the Departments based on the sortOrder parameter by DepartmentSortState.
            // The switch statement selects the appropriate LINQ method based on the sortOrder value.
            // If an invalid value is passed, the default case sorts by Surname.
            departmentsIQ = sortOrder switch
            {
                DepartmentSortState.NameDesc => departmentsIQ.OrderByDescending(x => x.Name),
                DepartmentSortState.FloorAsc => departmentsIQ.OrderBy(x => x.Floor),
                DepartmentSortState.FloorDesc => departmentsIQ.OrderByDescending(x => x.Floor),
                _ => departmentsIQ.OrderBy(_ => _.Name)
            };

            // Bind the sorted data to the Departments property, with AsNoTracking() to improve performance.
            Departments = await departmentsIQ.AsNoTracking().ToListAsync();
        }
    }
}
