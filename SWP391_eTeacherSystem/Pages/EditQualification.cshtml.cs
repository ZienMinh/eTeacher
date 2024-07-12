using BusinessObject.Models;
using DataAccess;
using eTeacher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using System.Linq;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Pages
{
	public class EditQualificationModel : PageModel
	{
		private readonly AddDbContext _context;
        private readonly IUserService _userService;
        private readonly ILogger<EditQualificationModel> _logger;
        private readonly IAuthService _authService;

		public EditQualificationModel(AddDbContext context, IUserService userService, 
            ILogger<EditQualificationModel> logger, IAuthService authService)
		{
			_context = context;
			_logger = logger;
            _userService = userService;
            _authService = authService;
		}

        public Qualification Qualification { get; set; }

        [BindProperty]
        public QualificationDto QualificationDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public string QualificationId { get; set; }

        /*public async Task InitializeClassDtoAsync()
        {
            var userId = _userService.GetCurrentUserId();
            if (userId != null)
            {
                var qualificationId = _userService.GenerateQualificationId();
                QualificationDto = new QualificationDto { User_id = userId, Qualification_id = qualificationId };
            }
        }*/
        public async Task<IActionResult> OnGetAsync(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Qua ID is not provided.");
            }

            QualificationId = id;

            Qualification = await _context.Qualifications.FirstOrDefaultAsync(c => c.Qualification_id == id);

            if (Qualification == null)
            {
                return NotFound();
            }

            var userId = _authService.GetCurrentUserId();
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync started");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is not valid");
                // Log chi tiết các lỗi trong ModelState
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogWarning($"Property: {state.Key} - Errors: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
                return Page();
            }

            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("User is not authenticated");
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            QualificationDto.User_id = userId;


            try
            {
                _logger.LogInformation("Attempting to create requirement");

                var result = await _userService.UpdateQualificationAsync(QualificationDto);
                if (result.IsSucceed)
                {
                    _logger.LogInformation("Qualification updated successfully");
                    return RedirectToPage("/TutorPage");
                }
                else
                {
                    _logger.LogWarning("Failed to create requirement: " + result.Message);
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while saving data: " + ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
            }

            return Page();
        }
    }
}
