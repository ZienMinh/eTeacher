using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class ClassDetailsModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IClassService _classService;
        private readonly IClassHourService _classHourService;
        private readonly ILogger<ClassDetailsModel> _logger;

        public ClassDetailsModel(AddDbContext context, IAuthService authService, 
            IUserService userService, IClassService classService, ILogger<ClassDetailsModel> logger, IClassHourService classHourService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
            _classService = classService;
            _logger = logger;
            _classHourService = classHourService;
        }

        public UserDto UserDto { get; set; }

        public ClassHour ClassHour { get; set; }

        [BindProperty]
        public ClassDto ClassDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClassId { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var classId = _classService.GenerateClassId();
                ClassDto = new ClassDto {
                    Tutor_id = ClassHour.User_id,
                    Student_id = userId, 
                    Class_id = classId 
                };
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

            ClassHour = await _context.ClassHours.FirstOrDefaultAsync(c => c.Class_id == id);

            if (ClassHour == null)
            {
                _logger.LogWarning("Class not found.");
                return NotFound();
            }

            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                ClassDto = new ClassDto { 
                    Tutor_id = ClassHour.User_id,
                    Student_id = userId
                };
            }

            _logger.LogInformation("OnGetAsync completed successfully.");
            await InitializeClassDtoAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("ClassId");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is not valid");
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogWarning($"Property: {state.Key} - Errors: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
                return Page();
            }

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
                _logger.LogInformation("Attempting to create class and delete class hour");
                var result = await _classService.CreateClassAsync(ClassDto, userId);
                if (result.IsSucceed)
                {
                    _logger.LogInformation("Deleting requirement with ID: {ID}", ClassId);

                    var delete = await _classHourService.DeleteClassAsync(ClassId);
                    if (delete.IsSucceed)
                    {
                        _logger.LogInformation("Class created and class hour deleted successfully");
                        return RedirectToPage("/StudentPage", new { id = result.CreatedClass.Class_id });
                    }
                    else
                    {
                        _logger.LogWarning("Failed to delete requirement: " + delete.Message);
                        ModelState.AddModelError(string.Empty, delete.Message);
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to create class: " + result.Message);
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
