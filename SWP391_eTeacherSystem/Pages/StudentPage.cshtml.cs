using BusinessObject.Models;
using DataAccess;
using eTeacher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class StudentPageModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IClassHourService _classHourService;

        public StudentPageModel(IUserService userService, IAuthService authService, IClassHourService classHourService)
        {
            _userService = userService;
            _authService = authService;
            _classHourService = classHourService;
        }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string SubjectName { get; set; }

        public List<UserDto> Tutors { get; set; }

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
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<IActionResult> OnPostSearchTutorAsync()
        {
            Tutors = await _userService.SearchTutorAsync(Name);
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            HasSearched = true;
            Classes = await _classHourService.SearchSubjectAsync(SubjectName);
            return Page();
        }
    }
}
