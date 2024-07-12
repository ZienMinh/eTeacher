using BusinessObject.Models;
using DataAccess;
using eTeacher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class CreateFeedbackModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<CreateFeedbackModel> _logger;

        public CreateFeedbackModel(IReportService reportService, AddDbContext context, IAuthService authService, ILogger<CreateFeedbackModel> logger )
        {
            _reportService = reportService;
            _context = context;
            _logger = logger;
            _authService = authService;
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
            _logger.LogInformation("OnGetAsync started with ID: {ID}", id);

            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Class ID is not provided.");
                return NotFound("Class ID is not provided.");
            }

            ClassId = id;

            Class = await _context.Classes.FirstOrDefaultAsync(c => c.Class_id == id);

            if (Class == null)
            {
                _logger.LogWarning("Class not found.");
                return NotFound();
            }

            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("Login to perform the function.");
            }

            _logger.LogInformation("OnGetAsync completed successfully.");
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
            _logger.LogInformation("OnPostAsync started.");

            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                Feedback.Student_id = userId;
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                return Page();
            }

            // Retrieve the Class entity again to ensure it's not null
            Class = await _context.Classes.FirstOrDefaultAsync(c => c.Class_id == ClassId);

            if (Class != null)
            {
                Feedback.Class_id = ClassId;
                Feedback.Tutor_id = Class.Tutor_id;
            }

            try
            {
                _logger.LogInformation("Creating report with ID: {ID}", ClassId);

                var result = await _reportService.CreateFeedbackAsync(Feedback);

                if (result.IsSucceed)
                {
                    _logger.LogInformation("Feedback created successfully.");
                    return RedirectToPage("/StudentPage");
                }
                else
                {
                    _logger.LogWarning("Feedback creation failed: {Message}", result.Message);
                    ModelState.AddModelError(string.Empty, result.Message);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while saving data: {Message}", ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
            }

            return Page();
        }
    }
}
