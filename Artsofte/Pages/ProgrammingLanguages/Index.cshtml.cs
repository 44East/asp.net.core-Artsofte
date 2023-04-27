using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Artsofte.Data;
using Artsofte.Models;

namespace Artsofte.Pages.ProgrammingLanguages
{
    public class IndexModel : PageModel
    {
        private readonly Artsofte.Data.ArtsofteContext _context;

        public IndexModel(Artsofte.Data.ArtsofteContext context)
        {
            _context = context;
        }

        public IList<ProgrammingLanguage> ProgrammingLanguage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.ProgrammingLanguages != null)
            {
                ProgrammingLanguage = await _context.ProgrammingLanguages.ToListAsync();
            }
        }
    }
}
