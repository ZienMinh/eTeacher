using eTeacher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class TutorPageModel : PageModel
    {
        private readonly IAuthService _authService;

        public TutorPageModel(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }
    }
}
