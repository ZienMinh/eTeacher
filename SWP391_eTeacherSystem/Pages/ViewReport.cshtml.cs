using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class ViewReportModel : PageModel
    {
        private readonly IReportService _reportService;

        public ViewReportModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        public List<ReportDto> Reports { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Class ID is not provided.");
            }

            ClassId = id;

            var feedbackResponse = await _reportService.GetReportAsync(ClassId);
            if (feedbackResponse.IsSucceed)
            {
                Reports = feedbackResponse.Reports;
            }
            else
            {
                Reports = new List<ReportDto>();
            }

            return Page();
        }

    }
}
