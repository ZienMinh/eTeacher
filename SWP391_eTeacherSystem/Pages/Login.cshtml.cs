using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public LoginDto LoginDto { get; set; }

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(LoginDto);
                if (result.IsSucceed)
                {
                    // Lýu token vào session ho?c cookie
                    HttpContext.Session.SetString("AccessToken", result.Message);
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return Page();
        }
    }
}
