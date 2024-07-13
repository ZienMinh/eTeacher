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

        public UserDto UserDto { get; set; }

        [BindProperty]
        public QualificationDto QualificationDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public string QualificationId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Qualification ID is not provided.");
                return NotFound("Qualification ID is not provided.");
            }

            _logger.LogInformation($"Retrieving qualification with ID: {id}");

            QualificationId = id;

            Qualification = await _context.Qualifications.FirstOrDefaultAsync(c => c.Qualification_id == id);

            if (Qualification == null)
            {
                _logger.LogWarning($"Qualification with ID {id} not found.");
                return NotFound($"Qualification with ID {id} not found.");
            }

            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("User is not authenticated.");
                return NotFound("User is not authenticated.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("User is not authenticated");
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            try
            {
                _logger.LogInformation($"Attempting to edit qualification with ID: {QualificationDto.Qualification_id}");
                var result = await _userService.UpdateQualificationAsync(QualificationDto);
                if (result.IsSucceed)
                {
                    _logger.LogInformation("Edit qualification successfully");
                    return RedirectToPage("/TutorProfile");
                }
                else
                {
                    _logger.LogWarning($"Failed to edit qualification: {result.Message}");
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while saving data: {ex.Message}");
                ModelState.AddModelError(string.Empty, $"An error occurred while saving data: {ex.Message}");
            }

            return Page();
        }


    }

}
