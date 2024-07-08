using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class ManageReportModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly AddDbContext _context;
        private readonly IClassService _classService;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ManageReportModel> _logger;

        public ManageReportModel(IReportService reportService, AddDbContext context,
            IClassService classService, IEmailService emailService, UserManager<User> userManager, ILogger<ManageReportModel> logger)
        {
            _reportService = reportService;
            _context = context;
            _classService = classService;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        public Report Report { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReportId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Class ID is not provided.");
            }

            ReportId = id;

            Report = await _context.Reports.Include(r => r.Tutor).Include(r => r.Student).FirstOrDefaultAsync(r => r.Report_id == id);

            if (Report == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState is invalid");
                    return Page();
                }

                _logger.LogInformation("Loading report with ID: {ReportId}", ReportId);
                Report = await _context.Reports.Include(r => r.Tutor).Include(r => r.Student).FirstOrDefaultAsync(r => r.Report_id == ReportId);
                if (Report == null)
                {
                    _logger.LogWarning("Report not found with ID: {ReportId}", ReportId);
                    return NotFound("Report not found");
                }

                if (action == "cancel")
                {
                    _logger.LogInformation("Processing cancel action for report ID: {ReportId}", ReportId);
                    var reportDto = new ReportDto()
                    {
                        Processing = 2,
                        Report_id = ReportId
                    };

                    var resultReport = await _reportService.UpdateReportAsync(reportDto);
                    if (resultReport.IsSucceed)
                    {
                        var classDto = new ClassDto
                        {
                            Status = 2,
                            Class_id = Report.Class_id
                        };

                        var result = await _classService.UpdateClassAsync(classDto);
                        if (result.IsSucceed)
                        {
                            var emailSubject = "Class Cancellation Notice";
                            var emailBody = $"Dear {Report.Tutor.First_name} {Report.Tutor.Last_name},\n\nWe are eTeacher moderators.\nWe received a student report from the class {Report.Class_id} " +
                                            $"that you are teaching that {Report.Title} with the content is {Report.Content}. \nAfter reviewing and evaluating, we have concluded that " +
                                            $"the student's report is correct. We have decided to cancel the class and pay the tuition, you will receive the corresponding tuition " +
                                            $"according to the number of hours taught.\r\nIf you are reported more than 3 times, we will be forced to delete your account from the " +
                                            $"system.\r\nThank you very much for using our service.";

                            var emailResult = await _reportService.SendEmailAsync(Report.Tutor_id, emailSubject, emailBody);
                            if (!emailResult.IsSucceed)
                            {
                                _logger.LogError("Failed to send email to tutor: {Message}", emailResult.Message);
                                ModelState.AddModelError(string.Empty, emailResult.Message);
                                return Page();
                            }

                            _logger.LogInformation("Report cancelled and email sent successfully for report ID: {ReportId}", ReportId);
                            return RedirectToPage("/ModeratorPage");
                        }
                        else
                        {
                            _logger.LogError("Failed to update class: {Message}", result.Message);
                            ModelState.AddModelError(string.Empty, result.Message);
                        }
                    }
                    else
                    {
                        _logger.LogError("Failed to update report: {Message}", resultReport.Message);
                        ModelState.AddModelError(string.Empty, resultReport.Message);
                    }
                }
                else if (action == "reject")
                {
                    _logger.LogInformation("Processing reject action for report ID: {ReportId}", ReportId);
                    var reportDto = new ReportDto()
                    {
                        Processing = 3,
                        Report_id = ReportId
                    };

                    var resultReport = await _reportService.UpdateReportAsync(reportDto);
                    if (resultReport.IsSucceed)
                    {
                        var emailSubject = "Report Rejection Notice";
                        var emailBody = $"Dear {Report.Student.First_name} {Report.Student.Last_name},\n\nWe are eTeacher moderators.\nWe received your report about the class {Report.Class_id} " +
                                        $"that you are studying that {Report.Title} with the content is {Report.Content}. \nAfter reviewing and evaluating, we believe that your report " +
                                        $"does not have enough evidence to conclude. We have decided not to cancel the class and if you want to stop studying, please select the cancel " +
                                        $"class function and your tuition fee will not be refunded if the number of lessons > 2 lessons.\r\nThank you very much for using our service.";

                        var emailResult = await _reportService.SendEmailAsync(Report.Student_id, emailSubject, emailBody);
                        if (!emailResult.IsSucceed)
                        {
                            _logger.LogError("Failed to send email to student: {Message}", emailResult.Message);
                            ModelState.AddModelError(string.Empty, emailResult.Message);
                            return Page();
                        }

                        _logger.LogInformation("Report rejected and email sent successfully for report ID: {ReportId}", ReportId);
                        return RedirectToPage("/ModeratorPage");
                    }
                    else
                    {
                        _logger.LogError("Failed to update report: {Message}", resultReport.Message);
                        ModelState.AddModelError(string.Empty, resultReport.Message);
                    }
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
