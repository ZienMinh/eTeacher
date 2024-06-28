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

        public StudentPageModel(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string SubjectName { get; set; }

        public List<UserDto> Tutors { get; set; }

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
    }
}
