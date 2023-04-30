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
        private readonly ModelsDataAccessLayer _models;
        /// <summary>
        /// Creates a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="models">The database context <see cref="ModelsDataAccessLayer"/>  for this page.</param>
        public IndexModel(ModelsDataAccessLayer models)
        {
            _models = models;
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
        public void OnGet(DepartmentSortState sortOrder = DepartmentSortState.NameAsc)
        {
            if (_models.Departments == null)
            {
                // If there are no Departments in the database, initialize an empty list and return.
                Departments = new List<Department>();
                return;
            }
            // Create an IEnumerable object for the Departments in the database.
            IEnumerable<Department> departments = _models.Departments;

            // Set up ViewData for the sort order. These values will be used in the Razor view to create links to sort the data.
            ViewData["NameSort"] = sortOrder == DepartmentSortState.NameAsc ? DepartmentSortState.NameDesc : DepartmentSortState.NameAsc;
            ViewData["FloorSort"] = sortOrder == DepartmentSortState.FloorAsc ? DepartmentSortState.FloorDesc : DepartmentSortState.FloorAsc;

            // Sort the Departments based on the sortOrder parameter by DepartmentSortState.
            // The switch statement selects the appropriate LINQ method based on the sortOrder value.
            // If an invalid value is passed, the default case sorts by Surname.
            departments = sortOrder switch
            {
                DepartmentSortState.NameDesc => departments.OrderByDescending(x => x.Name),
                DepartmentSortState.FloorAsc => departments.OrderBy(x => x.Floor),
                DepartmentSortState.FloorDesc => departments.OrderByDescending(x => x.Floor),
                _ => departments.OrderBy(_ => _.Name)
            };

            // Bind the sorted data to the Departments property.
            Departments = departments.ToList();
        }
    }
}
