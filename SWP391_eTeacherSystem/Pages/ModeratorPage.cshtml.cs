using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class ModeratorPageModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IReportService _reportService;

        public ModeratorPageModel(IAuthService authService, IReportService reportService)
        {
            _authService = authService;
            _reportService = reportService;
        }

        public ReportDto ReportDto { get; set; }

        public List<ReportDto> Reports { get; set; }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var reportResponse = await _reportService.GetAllReportAsync(ReportDto);
            if (reportResponse.IsSucceed)
            {
                Reports = reportResponse.Reports;
            }
            else
            {
                Reports = new List<ReportDto>();
            }
            return Page();

        }

    }
}
