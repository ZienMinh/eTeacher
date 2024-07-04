using BusinessObject.Models;
using DataAccess;
using eTeacher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_eTeacherSystem.Pages
{
    public class ViewFeedbackModel : PageModel
    {
        private readonly IReportService _reportService;

        public ViewFeedbackModel(IReportService reportService)
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

            var feedbackResponse = await _reportService.GetFeedbackAsync(ClassId);
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
