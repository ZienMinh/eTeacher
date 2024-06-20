using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

namespace SWP391_eTeacherSystem.Pages
{
    public class ClassModel : PageModel
    {
        private readonly IClassService _classService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<RequirementModel> _logger;

        public ClassModel(IClassService classService, IAuthService authService, AddDbContext context, ILogger<RequirementModel> logger)
        {
            _classService = classService;
            _authService = authService;
            _context = context;
            _logger = logger;
        }
        [BindProperty]
        public ClassDto ClassDto { get; set; }

        public List<Subject> Subjects { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var classId = _classService.GenerateClassId();
                ClassDto = new ClassDto { Tutor_id = userId, Class_id = classId };
            }
        }

        public async Task OnGetAsync()
        {
            Subjects = await _context.Subjects.ToListAsync();
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                ClassDto = new ClassDto { Tutor_id = userId };
            }
            await InitializeClassDtoAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
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
                await OnGetAsync();
                return Page();
            }

            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("User is not authenticated");
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            ClassDto.Tutor_id = userId;

            try
            {
                _logger.LogInformation("Attempting to create class hour");
                var result = await _classService.CreateClassAsync(ClassDto, userId);
                if (result.IsSucceed)
                {
                    _logger.LogInformation("Class hour created successfully");
                    return RedirectToPage("/ClassDetails", new { id = result.CreatedClass.Class_id });
                }
                else
                {
                    _logger.LogWarning("Failed to create class hour: " + result.Message);
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while saving data: " + ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
            }

            await OnGetAsync();
            return Page();
        }
    }
}
