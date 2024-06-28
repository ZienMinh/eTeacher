using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{

    public class AdminModel : PageModel
    {
        private readonly IAuthService _authService;

        public AdminModel(IAuthService authService)
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
