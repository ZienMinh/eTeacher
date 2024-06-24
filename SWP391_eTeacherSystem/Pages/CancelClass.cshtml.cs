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
    public class CancelClassModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IClassService _classService;
        private readonly ILogger<CancelClassModel> _logger;

        public CancelClassModel(AddDbContext context, IAuthService authService,
                IUserService userService, IClassService classService, ILogger<CancelClassModel> logger)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
            _classService = classService;
            _logger = logger;
        }

        public UserDto UserDto { get; set; }

        public Class Class { get; set; }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassId { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("Login to perform the function.");
            }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            _logger.LogInformation("OnGetAsync started with ID: {ID}", id);

            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Class ID is not provided.");
                return NotFound("Class ID is not provided.");
            }

            ClassId = id;

            Class = await _context.Classes.FirstOrDefaultAsync(c => c.Class_id == id);

            if (Class == null)
            {
                _logger.LogWarning("Class not found.");
                return NotFound();
            }

            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("Login to perform the function.");
            }

            _logger.LogInformation("OnGetAsync completed successfully.");
            await InitializeClassDtoAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("User is not authenticated");
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            ClassDto.Student_id = userId;

            try
            {
                _logger.LogInformation("Deleting class with ID: {ID}", ClassId);
                var result = await _classService.DeleteClassAsync(ClassId);
                if (result.IsSucceed)
                {
                    _logger.LogInformation("Class deleted successfully");
                    return RedirectToPage("/Index");
                }
                else
                {
                    _logger.LogWarning("Failed to delete class: " + result.Message);
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
