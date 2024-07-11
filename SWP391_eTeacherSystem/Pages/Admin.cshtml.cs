using BusinessObject.Models;
using DataAccess;
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
        public FeeDto FeeDto { get; set; }

        public async Task OnGetAsync()
        {
            VisitorCount = _visitorCounterService.GetVisitorCount();
            RegisteredUserCount = _visitorCounterService.GetRegisteredUserCount();
            RequirementCount = _visitorCounterService.GetRequirementCount();
            ReportCount = _visitorCounterService.GetReportCount();

            var fees = await _context.Fees
            .OrderBy(f => f.Date)
            .ToListAsync();

            FeeDto = new FeeDto
            {
                Labels = fees.Select(f => f.Date.ToString("MM/dd/yyyy")).ToList(),
                TotalFeesData = fees.Select(f => f.TotalFees).ToList(),
                PlatformFeesData = fees.Select(f => f.PlatformFees).ToList()
            };

        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }
    }
}
