using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;

        public ResetPasswordModel(IAuthService authService, AddDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [BindProperty]
        public string userName { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var check = await _authService.ResetPasswordByEmailAsync(userName);
                if (check.IsSucceed)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("", check.Message);
                }
            }
            return Page();
        }
    }
}
