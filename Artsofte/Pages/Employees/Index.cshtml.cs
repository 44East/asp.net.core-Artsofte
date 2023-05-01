using Microsoft.AspNetCore.Mvc.RazorPages;
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
        /// The list of <see cref="Models.Employee"/> to display on the index page.
        /// </summary>
        public IList<Employee> Employees { get; set; }

        /// <summary>
        /// Receives a collection of <see cref="Models.Employee"/> from the <see cref="ModelsDataAccessLayer"/> and sorts them based on the specified <see cref="EmployeeSortState"/> states.
        /// </summary>
        /// <param name="sortOrder">The sorting order for the employee data. Default value is <see cref="EmployeeSortState.SurnameAsc"/>.</param>
        public void OnGet(EmployeeSortState sortOrder = EmployeeSortState.SurnameAsc)
        {
            if (_models == null && !_models.IsDBExist)
            {
                // If there are no Employees in the database, initialize an empty list and return.
                Employees = new List<Employee>();
                return;
            }
            // Create an IEnumerable object for the Employees in the database.
            IEnumerable<Employee> employees = _models.Employees;

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
            employees = sortOrder switch
            {
                EmployeeSortState.NameAsc => employees.OrderBy(e => e.Name),
                EmployeeSortState.NameDesc => employees.OrderByDescending(e => e.Name),
                EmployeeSortState.SurnameDesc => employees.OrderByDescending(e => e.Surname),
                EmployeeSortState.AgeAsc => employees.OrderBy(e => e.Age),
                EmployeeSortState.AgeDesc => employees?.OrderByDescending(e => e.Age),
                EmployeeSortState.GenderAsc => employees.OrderBy(e => e.Gender),
                EmployeeSortState.GenderDesc => employees.OrderByDescending(e => e.Gender),
                EmployeeSortState.DepartmentAsc => employees.OrderBy(e => e.Department.Name),
                EmployeeSortState.DepartmentDesc => employees.OrderByDescending(e => e.Department.Name),
                EmployeeSortState.ProgLangAsc => employees.OrderBy(e => e.ProgrammingLanguage.Name),
                EmployeeSortState.ProgLangDesc => employees.OrderByDescending(e => e.ProgrammingLanguage.Name),
                _ => employees.OrderBy(e => e.Surname)
            };

            // Bind the sorted data to the Employees property.
            Employees = employees.ToList();           


        }
    }
}
