using Artsofte.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artsofte.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ModelsDataAccessLayer _models;
        public IndexModel(ModelsDataAccessLayer modelsDAL)
        {
            _models = modelsDAL;
        }
        public async Task OnGet()
        {
            await _models.CheckDataBaseStatusAsync();
        }
    }
}
