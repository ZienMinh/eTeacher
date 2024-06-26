using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace SWP391_eTeacherSystem.Pages
{

    public class AdminModel : PageModel
    {
        private readonly VisitorCounterService _visitorCounterService = new VisitorCounterService();

        public int TotalVisitors { get; set; }
        public void OnGet()
        {
            TotalVisitors = _visitorCounterService.TotalVisitors;
        }
    }
}
