using BusinessObject.Models;
using DataAccess;
using eTeacher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class CreateQualificationModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ILogger<CreateQualificationModel> _logger;

        public CreateQualificationModel(IUserService userService, IAuthService authService, ILogger<CreateQualificationModel> logger)
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public QualificationDto QualificationDto { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var qualificationId = _userService.GenerateQualificationId();
                QualificationDto = new QualificationDto
                {
                    User_id = userId,
                    Qualification_id = qualificationId,
                };
            }
        }

        public async Task OnGetAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                QualificationDto = new QualificationDto { User_id = userId };
            }
            await InitializeClassDtoAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            QualificationDto.User_id = userId;

            try
            {
                var result = await _userService.CreateQualificationAsync(QualificationDto);
                if (result.IsSucceed)
                {
                    return RedirectToPage("/TutorPage", new { id = result.CreatedQualification.Qualification_id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
            }
            await OnGetAsync();
            return Page();
        }


    }
}
