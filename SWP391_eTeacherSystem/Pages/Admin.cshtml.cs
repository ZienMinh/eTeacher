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
        private readonly IVisitorCounterService _visitorCounterService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;

        public int VisitorCount { get; private set; }
        public int RegisteredUserCount { get; private set; }
        public int RequirementCount { get; private set; }
        public int ReportCount { get; private set; }


        public AdminModel(IAuthService authService, IVisitorCounterService visitorCounterService,AddDbContext context)
        {
            _context = context;
            _authService = authService;
            _visitorCounterService = visitorCounterService;
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

    //    public IList<DataPoint> DataPoints { get; set; }
    //    public List<int> ThisWeekData { get; set; }
    //    public List<int> LastWeekData { get; set; }
    //    public List<string> Labels { get; set; } // e.g., Dates or Days

    //    public async Task OnGetAsync()
    //    {
    //        var now = DateTime.UtcNow;
    //        var startOfWeek = now.StartOfWeek(DayOfWeek.Monday);
    //        var startOfLastWeek = startOfWeek.AddDays(-7);
    //        var endOfLastWeek = startOfWeek.AddDays(-1);

    //        DataPoints = await _context.DataPoints
    //            .Where(dp => dp.Date >= startOfLastWeek && dp.Date < startOfWeek)
    //            .OrderBy(dp => dp.Date)
    //            .ToListAsync();

    //        ThisWeekData = DataPoints
    //            .Where(dp => dp.Date >= startOfWeek)
    //            .GroupBy(dp => dp.Date.Date)
    //            .Select(g => g.Sum(dp => dp.PlatformFees))
    //            .ToList();

    //        LastWeekData = DataPoints
    //            .Where(dp => dp.Date >= startOfLastWeek && dp.Date < startOfWeek)
    //            .GroupBy(dp => dp.Date.Date)
    //            .Select(g => g.Sum(dp => dp.PlatformFees))
    //            .ToList();

    //        // Generate labels for the x-axis
    //        Labels = Enumerable.Range(0, Math.Max(ThisWeekData.Count, LastWeekData.Count))
    //            .Select(i => startOfWeek.AddDays(i).ToString("dd/MM"))
    //            .ToList();
    //    }
    //}
    public int UserCount { get; set; }
        public void OnGet()
        {
            RegisteredUserCount = _visitorCounterService.GetRegisteredUserCount();
            RequirementCount = _visitorCounterService.GetRequirementCount();
            ReportCount = _visitorCounterService.GetReportCount();
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }
    }
}
