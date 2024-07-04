using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class CreateReportModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;

        public CreateReportModel(IReportService reportService, IAuthService authService, AddDbContext context)
        {
            _reportService = reportService;
            _authService = authService;
            _context = context;
        }

        public Class Class { get; set; }

        public ClassDto ClassDto { get; set; }

        [BindProperty]
        public ReportDto Feedback { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassId { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var reportId = _reportService.GenerateReportId();
                Feedback = new ReportDto { Student_id = userId, Report_id = reportId };
            }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Class ID is not provided.");
            }

            ClassId = id;

            Class = await _context.Classes.FirstOrDefaultAsync(c => c.Class_id == id);

            if (Class == null)
            {
                return NotFound();
            }

            var userId = _authService.GetCurrentUserId();

            await InitializeClassDtoAsync();

            if (Class != null)
            {
                Feedback.Tutor_id = Class.Tutor_id;
                Feedback.Class_id = ClassId;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                Feedback.Student_id = userId;
            }

            Class = await _context.Classes.FirstOrDefaultAsync(c => c.Class_id == ClassId);

            if (Class != null)
            {
                Feedback.Class_id = ClassId;
                Feedback.Tutor_id = Class.Tutor_id;
            }

            try
            {

                var result = await _reportService.CreateFeedbackAsync(Feedback);

                if (result.IsSucceed)
                {
                    return RedirectToPage("/StudentPage");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
            }

            return Page();
        }
    }
}
