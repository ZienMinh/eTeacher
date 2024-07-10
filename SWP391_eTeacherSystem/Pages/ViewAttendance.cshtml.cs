using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class ViewAttendanceModel : PageModel
    {
        private readonly IAttendanceService _attendanceService;

        public ViewAttendanceModel(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [BindProperty(SupportsGet = true)]
        public string ClassId { get; set; }

        public List<AttendanceDto> Attendances { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Class ID is not provided.");
            }

            ClassId = id;

            var response = await _attendanceService.GetAttendanceAsync(ClassId);
            if (response.IsSucceed)
            {
                Attendances = response.Data;
            }
            else
            {
                Attendances = new List<AttendanceDto>();
                ModelState.AddModelError(string.Empty, response.Message);
            }
            return Page();
        }
    }
}
