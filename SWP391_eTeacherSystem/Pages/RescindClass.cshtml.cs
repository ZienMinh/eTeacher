using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using System;
using System.Threading.Tasks;

namespace SWP391_eTeacherSystem.Pages
{
    public class RescindClassModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IClassService _classService;
        private readonly IAttendanceService _attendanceService;

        public RescindClassModel(AddDbContext context, IAuthService authService, IClassService classService, IAttendanceService attendanceService)
        {
            _context = context;
            _authService = authService;
            _classService = classService;
            _attendanceService = attendanceService;
        }

        public Class Class { get; set; }

        [BindProperty]
        public AttendanceDto AttendanceDto { get; set; }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassId { get; set; }

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

            AttendanceDto = new AttendanceDto
            {
                Class_id = ClassId
            };

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            ClassDto.Student_id = userId;

            try
            {
                var classDto = new ClassDto
                {
                    Class_id = ClassId,
                    Status = 2
                };

                var result = await _classService.UpdateClassAsync(classDto);
                if (result.IsSucceed)
                {
                    return RedirectToPage("/StudentPage");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            AttendanceDto.Class_id = ClassId; // Ensure Class_id is set

            switch (action)
            {
                case "presence":
                    AttendanceDto.Status = 2;
                    break;
                case "absence":
                    AttendanceDto.Status = 3;
                    break;
                default:
                    AttendanceDto.Status = 1; // Default status
                    break;
            }

            var response = await _attendanceService.CreateAttendanceAsync(AttendanceDto);

            if (!response.IsSucceed)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                Class = await _context.Classes.FirstOrDefaultAsync(c => c.Class_id == ClassId);
                return Page();
            }

            return RedirectToPage(new { id = ClassId });
        }
    }
}
