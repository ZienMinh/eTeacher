using BusinessObject.Models;
using DataAccess;
using eTeacher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services;
using Services.Interfaces;

namespace SWP391_eTeacherSystem.Pages
{
    public class StudentPageModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IAcademicVideoService _videoService;
        private readonly IScheduleService _scheduleService;

        public StudentPageModel(IUserService userService, IAuthService authService, IAcademicVideoService videoService, IScheduleService scheduleService)
        {
            _userService = userService;
            _authService = authService;
            _videoService = videoService;
            _scheduleService = scheduleService;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string SubjectName { get; set; }

        public List<UserDto> Tutors { get; set; }

		public IEnumerable<AcademicVideo> AcademicVideos { get; set; }
        public IEnumerable<Schedule> Schedules { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Tutors = await _userService.SearchTutorAsync(Name, SubjectName);
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }

		public async Task<IActionResult> OnGetAsync()
		{
			var userId = _authService.GetCurrentUserId();
			if (userId == null)
			{
				return Unauthorized();
			}

			AcademicVideos = await _videoService.GetAllVideosAsync();
            Schedules = await _scheduleService.GetSchedulesByStudentIdAsync(userId);
            return Page();
		}
	}
}
