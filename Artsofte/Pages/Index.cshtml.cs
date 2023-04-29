using Artsofte.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artsofte.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ModelsDAL _models;
        public IndexModel(ModelsDAL modelsDAL)
        {
            _models = modelsDAL;
        }
        public async Task OnGet()
        {
            await _models.CheckDataBaseStatus();
        }
    }
}
