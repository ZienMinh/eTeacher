using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly VisitorCounterService _visitorCounterService = new VisitorCounterService();
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _visitorCounterService.IncrementVisitorCount();
        }

      
    }
}
