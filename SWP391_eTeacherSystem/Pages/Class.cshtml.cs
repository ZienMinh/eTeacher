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
        private readonly IClassHourService _classhourService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<RequirementModel> _logger;

        private const double PricePerHour = 100000;

        public ClassModel(IClassHourService classhourService, IAuthService authService, AddDbContext context, ILogger<RequirementModel> logger)
        {
            _classhourService = classhourService;
            _authService = authService;
            _context = context;
            _logger = logger;
        }
        [BindProperty]
        public ClassHourDto ClassHourDto { get; set; }

        public List<Subject> Subjects { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var classId = _classhourService.GenerateClassHourId();
                ClassHourDto = new ClassHourDto {
                    User_id = userId, 
                    Class_id = classId,
                    Price = PricePerHour
                };
            }
        }

        public async Task OnGetAsync()
        {
            Subjects = await _context.Subjects.ToListAsync();
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                ClassHourDto = new ClassHourDto { User_id = userId };
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

            ClassHourDto.User_id = userId;

            ClassHourDto.Price = PricePerHour;
            var startTime = ClassHourDto.Start_time.Value;
            var endTime = ClassHourDto.End_time.Value;
            var duration = (endTime - startTime).TotalHours;
            ClassHourDto.Total = ClassHourDto.Number_of_session * duration * PricePerHour;

            try
            {
                _logger.LogInformation("Attempting to create class hour");
                var result = await _classhourService.CreateClassAsync(ClassHourDto, userId);
                if (result.IsSucceed)
                {
                    _logger.LogInformation("Class hour created successfully");
                    return RedirectToPage("/TutorPage", new { id = result.CreatedClass.Class_id });
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
