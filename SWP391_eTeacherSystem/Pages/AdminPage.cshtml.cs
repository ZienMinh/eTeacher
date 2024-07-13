using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{

    public class AdminModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;

        public int VisitorCount { get; private set; }
        public int RegisteredUserCount { get; private set; }
        public int RequirementCount { get; private set; }
        public int ReportCount { get; private set; }


        public AdminModel(IAuthService authService,AddDbContext context)
        {
            _context = context;
            _authService = authService;
        }
        //public List<string> Labels { get; set; }
        //public List<int> TotalFeesData { get; set; }
        //public List<int> PlatformFeesData { get; set; }

        //public void OnGet()
        //{
        //    var dataPoints = _context.DataPoints.OrderBy(dp => dp.Date).ToList();
        //    Labels = dataPoints.Select(dp => dp.Date.ToString("MMM")).ToList(); // e.g., 'JUN', 'JUL', 'AUG', ...
        //    TotalFeesData = dataPoints.Select(dp => dp.TotalFees).ToList();
        //    PlatformFeesData = dataPoints.Select(dp => dp.PlatformFees).ToList();
        //}

    public int UserCount { get; set; }
        public void OnGet()
        {
            RegisteredUserCount = _context.Users.Count();
            RequirementCount = _context.Requirements.Count();
            ReportCount = _context.Reports.Count();
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }
    }
}
