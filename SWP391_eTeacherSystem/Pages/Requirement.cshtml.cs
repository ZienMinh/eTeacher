using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Linq;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Pages
{
    public class RequirementModel : PageModel
    {

        private readonly AddDbContext _context;
        private readonly IRequirementService _requirementService;
        private readonly IAuthService _authService;
        private readonly ILogger<RequirementModel> _logger;

        private const double PricePerHour = 100000;

        public RequirementModel(AddDbContext context, IAuthService authService, IRequirementService requirementService, ILogger<RequirementModel> logger)
        {
            _context = context;
            _authService = authService;
            _requirementService = requirementService;
            _logger = logger;
        }

        [BindProperty]
        public RequirementDto RequirementDto { get; set; }

        public List<Subject> Subjects { get; set; }


        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var requirementId = _requirementService.GenerateRequirementId();
                RequirementDto = new RequirementDto
                {
                    User_id = userId,
                    Requirement_id = requirementId,
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
                RequirementDto = new RequirementDto { User_id = userId };
            }
            await InitializeClassDtoAsync();
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

            RequirementDto.User_id = userId;

            // Tính toán Price và Total
            RequirementDto.Price = PricePerHour;
            var startTime = RequirementDto.Start_time.Value;
            var endTime = RequirementDto.End_time.Value;
            var duration = (endTime - startTime).TotalHours;
            RequirementDto.Total = RequirementDto.Number_of_session * duration * PricePerHour;


            try
            {
                _logger.LogInformation("Attempting to create requirement");
                var result = await _requirementService.CreateRequirementAsync(RequirementDto, userId);
                if (result.IsSucceed)
                {
                    _logger.LogInformation("Requirement created successfully");
                    return RedirectToPage("/StudentPage", new { id = result.CreatedRequirement.Requirement_id });
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

            await OnGetAsync();
            return Page();
        }


    }
}
