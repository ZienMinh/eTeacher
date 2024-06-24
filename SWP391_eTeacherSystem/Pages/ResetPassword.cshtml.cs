using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Repositories;

namespace SWP391_eTeacherSystem.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<ResetPasswordModel> _logger;

        public ResetPasswordModel(IAuthService authService, AddDbContext context, ILogger<ResetPasswordModel> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
        }


        [BindProperty]
        public ResetPasswordDto ResetPasswordDto { get; set; }

        [BindProperty]
        public UserDto UserDto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Attempting to reset password for user: {ResetPasswordDto.UserName}");

                var check = await _authService.ResetPasswordByEmailAsync(ResetPasswordDto);
                if (check.IsSucceed)
                {
                    _logger.LogInformation("Password reset succeeded, redirecting to index page.");
                    return RedirectToPage("/Index");
                }
                else
                {
                    _logger.LogWarning($"Password reset failed: {check.Message}");
                    ModelState.AddModelError("", check.Message);
                }
            }
            return Page();
        }

    }


}
