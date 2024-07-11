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
        private readonly IClassHourService _classHourService;

        public StudentPageModel(IUserService userService, IAuthService authService, IAcademicVideoService videoService, IScheduleService scheduleService, IClassHourService classHourService)
        {
            _userService = userService;
            _authService = authService;
            _videoService = videoService;
            _scheduleService = scheduleService;
            _classHourService = classHourService;
        }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string SubjectName { get; set; }

        public List<UserDto> Tutors { get; set; }

        public IEnumerable<AcademicVideo> AcademicVideos { get; set; }
        public IEnumerable<Schedule> Schedules { get; set; }

        public List<ClassHour> Classes { get; set; }

        public bool HasSearched { get; set; } = false;

        public async Task OnGetAsync()
        {
            try
            {
                var response = await _classHourService.GetAll(new ClassHourDto());
                Classes = response.Classes.OrderBy(c => c.Status).ToList();
                var userId = _authService.GetCurrentUserId();
                if (userId != null)
                {
                    ClassDto = new ClassDto { Student_id = userId };
                    AcademicVideos = await _videoService.GetAllVideosAsync();
                    Schedules = await _scheduleService.GetSchedulesByStudentIdAsync(userId);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        public async Task<IActionResult> OnPostSearchTutorAsync()
        {
            Tutors = await _userService.SearchTutorAsync(Name);
            await LoadAcademicVideosAndSchedules();
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }

        private async Task LoadAcademicVideosAndSchedules()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                AcademicVideos = await _videoService.GetAllVideosAsync();
                Schedules = await _scheduleService.GetSchedulesByStudentIdAsync(userId);
            }
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            HasSearched = true;
            Classes = await _classHourService.SearchSubjectAsync(SubjectName);
            return Page();
        }
    }
}
