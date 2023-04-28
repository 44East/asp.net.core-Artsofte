using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;
using Artsofte.Models.SortingStates;

namespace Artsofte.Pages.ProgrammingLanguages
{
    /// <summary>
    /// The page model for showing all <see cref="Models.ProgrammingLanguage"/> objects.
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
        /// The list of <see cref="Models.ProgrammingLanguage"/> to display on the index page.
        /// </summary>
        public IList<ProgrammingLanguage> ProgrammingLanguages { get;set; }

        /// <summary>
        /// Retrieves a list of <see cref="Models.ProgrammingLanguage"/> from the database and sorts them based on the specified <see cref="ProgLanguageSortState"/> states.
        /// </summary>
        /// <param name="sortOrder">The sorting order for the employee data. Default value is <see cref="ProgLanguageSortState.NameAsc"/>.</param>
        /// <returns>A task that represents the asynchronous operation of retrieving and sorting employees.</returns>
        public async Task OnGetAsync(ProgLanguageSortState sortOrder = ProgLanguageSortState.NameAsc)
        {
            if (_context.ProgrammingLanguages == null)
            {
                // If there are no Programming Languages in the database, initialize an empty list and return.
                ProgrammingLanguages = new List<ProgrammingLanguage>();
                return;
            }

            // Create an IQueryable object for the Programming Languages in the database.
            IQueryable<ProgrammingLanguage> languagesIQ = _context.ProgrammingLanguages;

            // Set up ViewData for the sort order. These values will be used in the Razor view to create links to sort the data.
            ViewData["NameSort"] = sortOrder == ProgLanguageSortState.NameAsc ? ProgLanguageSortState.NameDesc : ProgLanguageSortState.NameAsc;

            // Sort the Prog. languages based on the sortOrder parameter by ProgLanguageSortState.
            // The switch statement selects the appropriate LINQ method based on the sortOrder value.
            // If an invalid value is passed, the default case sorts by Name.
            languagesIQ = sortOrder switch
            {
                ProgLanguageSortState.NameDesc => languagesIQ.OrderByDescending(l => l.Name),
                _ => languagesIQ.OrderBy(l => l.Name)
            };

            // Bind the sorted data to the ProgrammingLanguages property, with AsNoTracking() to improve performance.
            ProgrammingLanguages = await languagesIQ.AsNoTracking().ToListAsync();
        }
    }
}
