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
                    // Lưu token vào session hoặc cookie
                    HttpContext.Session.SetString("AccessToken", result.Message);

                    // Điều hướng dựa trên role
                    switch (result.Role)
                    {
                        case 1:
                            return RedirectToPage("/StudentPage");
                        case 2:
                            return RedirectToPage("/AdminPage");
                        case 3:
                            return RedirectToPage("/TutorPage");
                        case 4:
                            return RedirectToPage("/ModeratorPage");
                        default:
                            return RedirectToPage("/Index");
                    }
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
