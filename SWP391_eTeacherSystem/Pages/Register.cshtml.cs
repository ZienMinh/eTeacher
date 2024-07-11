using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Repositories;


namespace SWP391_eTeacherSystem.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public RegisterModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public RegisterDto RegisterDto { get; set; }

        [BindProperty]
        public QualificationDto QualificationDto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _authService.RegisterAsync(RegisterDto);
            if (result.IsSucceed)
            {
                /*QualificationDto = new QualificationDto
                {
                    Qualification_id = _userService.GenerateQualificationId(),
                };
                var resultQua = await _userService.CreateQualificationAsync(QualificationDto);*/
                return RedirectToPage("/Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }
        }
    }
}
